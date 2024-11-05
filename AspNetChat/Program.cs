using AspNetChat.Core.Entities;
using CommandLine;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Text;
using AspNetChat.Core.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Chat;
using Chat.Interfaces.Services;
using Common.Extensions.DI;
using Mongo;

namespace AspNetChat;

public class Program
{
	public static void Main(string[] args)
	{
		Parser.Default.ParseArguments<Options>(args)
			.WithParsed(options => CreateAspNetApp(args, options));
	}

	private static void ConfigureHttpsCertificates(WebApplicationBuilder builder, Options options) 
	{
		if (!options.UseKestrel)
			return;
			
		builder.WebHost.UseKestrel(kestrelServerOptions =>
		{
			kestrelServerOptions.Listen(IPAddress.Any, options.HttpPort ?? Constants.Ports.HttpPort);
				
			if (!options.CustomHttpsCertificate)
				return;
				
			var httpsSettings = builder.Configuration.GetSection("Https").Get<HttpsCertificateSettings>();

			if (httpsSettings == null)
				throw new InvalidOperationException("unable to get https certificate configuration");
				
			kestrelServerOptions.Listen(IPAddress.Any, options.HttpsPort ?? Constants.Ports.HttpsPort, listenOptions =>
			{
				listenOptions.UseHttps(httpsSettings.GetCertificate());
			});
		});
	}

	private static void LoadJsons(WebApplicationBuilder builder, Options options) 
	{
		if (options.Jsons == null)
			return;

		var sb = new StringBuilder();

		foreach (var item in options.Jsons)
		{
			var fullPath = Path.GetFullPath(item);

			var provider = new PhysicalFileProvider(Path.GetDirectoryName(fullPath)!);

			builder.Configuration.AddJsonFile(provider, Path.GetFileName(fullPath), false, true);

			sb.AppendLine(item);
		}

		Console.WriteLine($"loaded jsons:\n{sb}");
	}

	private static void SetStaticFilesLocation(WebApplication app, Options options) 
	{
		if (string.IsNullOrWhiteSpace(options.StaticFilesLocation))
		{
			app.UseStaticFiles();
		}
		else 
		{
			if (!Directory.Exists(options.StaticFilesLocation))
				throw new InvalidOperationException($"directory with static files is not exists: {options.StaticFilesLocation}");

			app.UseStaticFiles(
				new StaticFileOptions(
					new SharedOptions() 
					{ 
						FileProvider = new PhysicalFileProvider(options.StaticFilesLocation)
					}));
		}
	}

	private static void CreateAspNetApp(string[] args, Options options) 
	{
		var builder = WebApplication.CreateBuilder(args);

		LoadJsons(builder, options);

		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
		
		builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
		{
			new MainInstaller(containerBuilder).Install();
			
			new ChatInstaller(containerBuilder).Install();
			
			var dbConnection = options.DataBaseConnection.ToArray();
			new MongoInstaller(containerBuilder, dbConnection[0], dbConnection[1]).Install();
		});
		
		// Add services to the container.
		builder.Services.AddRazorPages();
			
		ConfigureHttpsCertificates(builder, options);
			
		// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
		// builder.Services.AddDbContext<ApplicationDbContext>(options =>
		// 	options.UseSqlServer(connectionString));
		// builder.Services.AddDatabaseDeveloperPageExceptionFilter();
		//
		// builder.Services.AddDefaultIdentity<IdentityUser>(identityOptions => identityOptions.SignIn.RequireConfirmedAccount = true)
		// 	.AddEntityFrameworkStores<ApplicationDbContext>();
		// builder.Services.AddRazorPages();
		//
		// builder.Services.Configure<IdentityOptions>(identityOptions =>
		// {
		// 	// Password settings.
		// 	// identityOptions.Password.RequireDigit = true;
		// 	// identityOptions.Password.RequireLowercase = true;
		// 	// identityOptions.Password.RequireNonAlphanumeric = true;
		// 	// identityOptions.Password.RequireUppercase = true;
		// 	identityOptions.Password.RequiredLength = 6;
		// 	// identityOptions.Password.RequiredUniqueChars = 1;
		//
		// 	// Lockout settings.
		// 	identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
		// 	identityOptions.Lockout.MaxFailedAccessAttempts = 5;
		// 	identityOptions.Lockout.AllowedForNewUsers = true;
		//
		// 	// User settings.
		// 	identityOptions.User.AllowedUserNameCharacters =
		// 		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
		// 	// identityOptions.User.RequireUniqueEmail = false;
		// });
		//
		// builder.Services.ConfigureApplicationCookie(cookieAuthenticationOptions =>
		// {
		// 	// Cookie settings
		// 	// cookieAuthenticationOptions.Cookie.HttpOnly = true;
		// 	cookieAuthenticationOptions.ExpireTimeSpan = TimeSpan.FromMinutes(5);
		//
		// 	cookieAuthenticationOptions.LoginPath = "/Identity/Account/Login";
		// 	cookieAuthenticationOptions.AccessDeniedPath = "/Identity/Account/AccessDenied";
		// 	cookieAuthenticationOptions.SlidingExpiration = true;
		// });

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseWebSockets();

		app.Map("/ChatHandler/{chatId}",
			async (string chatId, string userId, HttpContext context, IMessageListPublisherService messageListPublisherService) =>
			{
				await messageListPublisherService.ConnectWebSocket(userId, chatId, context);
			});

		app.MapPost("/UserDisconnected/{chatId}",
			async (string chatId, string userId, HttpContext context, IDisconnectionService disconnectionService) =>
			{
				await disconnectionService.DisconnectUser(userId, chatId, context);
			});

		app.MapPost("/UserSendMessage/{chatId}",
			async (
				string chatId,
				string userId,
				string message,
				HttpContext context,
				IMessageReceiverService messageReceiverService) =>
			{
				await messageReceiverService.ReceiveMessage(userId, chatId, message, context);
			});

		#region test
			
		//app.Map(new PathString("/test/{data}"), (IApplicationBuilder app) =>
		//{
		//	app.UseMiddleware<MessageListPublisherService>();
		//});
		//         app.UseWhen(
		//context => context.Request.Path == "/time", // если путь запроса "/time"
		//appBuilder =>
		//{
		//	// логгируем данные - выводим на консоль приложения
		//	appBuilder.Use(async (context, next) =>
		//	{
		//		var time = DateTime.Now.ToShortTimeString();
		//		Console.WriteLine($"Time: {time}");
		//		await next();   // вызываем следующий middleware
		//	});

		//	// отправляем ответ
		//	appBuilder.Run(async context =>
		//	{
		//		var time = DateTime.Now.ToShortTimeString();
		//		await context.Response.WriteAsync($"Time: {time}");
		//	});
		//});

		app.MapGet("/routes",
			(IEnumerable<EndpointDataSource> endpointSources) => string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));

		//app.Map("/users/{id:int:range(0, 1000)}", (int id) => $"User Id: {id}");

		//app.Map("/users/params", (int id) => $"User params Id: {id}");

		//app.Map(
		//	"map2/{controller=Home}/{action=Index}/{id?}",
		//	(string controller, string action, string? id) =>
		//		$"Controller: {controller} \nAction: {action} \nId: {id}"
		//);

		//app.Map("map3/{**info}", (string? info) => $"map3 info: {info ?? "n/a"}");

		//app.Map("map4/{*info}", (string? info) => $"map4 info: {info ?? "n/a"}");

		#endregion

		app.UseHttpsRedirection();
		SetStaticFilesLocation(app, options);

		app.UseRouting();

		// app.UseAuthentication();
		// app.UseAuthorization();
			
		app.MapRazorPages();
		
		app.Run();

		// var appDecoratorFactory = app.Services.GetService<IFactory<WebApplication, App>>()!;
		// using var appDecorator = appDecoratorFactory.Create(app);
		// 		
		// appDecorator.Initialize();
		// appDecorator.Run();
	}
}