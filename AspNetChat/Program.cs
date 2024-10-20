using AspNetChat.Core.Entities;
using AspNetChat.Core.Entities.Model;
using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Core.Services;
using AspNetChat.Core.Services.System;
using AspNetChat.Extensions;
using CommandLine;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Text;

namespace AspNetChat
{
	public class Program
    {
		public static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options => CreateAspNetApp(args, options));
		}

		private static void ConfigureHttpsCertificates(WebApplicationBuilder builder, Options options) 
		{
			if (!options.CustomHttpsCertificate)
				return;

			var httpsSettings = builder.Configuration.GetSection("Https").Get<HttpsCertificateSettings>();

			if (httpsSettings == null)
				throw new InvalidOperationException("unable to get https certificate configuration");

			builder.WebHost.UseKestrel(options =>
			{
				options.Listen(IPAddress.Any, 8080);
				options.Listen(IPAddress.Any, 8081, listenOptions =>
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

			Console.WriteLine($"loaded jsons:\n{sb.ToString()}");
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

			builder.Services.AddSingleton<DisposeService>();
			builder.Services.AddSingleton<InitializeService>();

			builder.Services.AddSingleton<IChatContainer, ChatDataModel>();
			builder.Services.AddFactoryFromMethod<ChatFactory.ChatParams, IChat>(
				(serviceProvider, arg1) =>
				{
					return new ChatModel(arg1.Guid, serviceProvider.GetService<IMessageConsumerService>()!);
				});
			builder.Services.AddSingleton<IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant>, ParticipantFactory>();
			builder.Services.BindSingletonInterfacesTo<MessageListPublisherService>();
			builder.Services.AddSingleton<ChatUserHelper>();
			builder.Services.BindSingletonInterfacesTo<DisconnectionService>();
			builder.Services.BindSingletonInterfacesTo<MessageReceiverService>();
			builder.Services.AddSingleton<ChatEventComposer>();

			// Add services to the container.
			builder.Services.AddRazorPages();

			ConfigureHttpsCertificates(builder, options);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseWebSockets();

			app.Map("/ChatHandler/{chatID}",
				async (string chatID, string userID, HttpContext context, IMessageListPublisherService messageListPublisherService) =>
				{
					await messageListPublisherService.ConnectWebSocket(userID, chatID, context);
				});

			app.MapPost("/UserDisconnected/{chatID}",
				async (string chatID, string userID, HttpContext context, IDisconnectionService disconnectionService) =>
				{
					await disconnectionService.DisconnectUser(userID, chatID, context);
				});

			app.MapPost("/UserSendMessage/{chatID}",
				async (
					string chatID,
					string userID,
					string message,
					HttpContext context,
					IMessageReceiverService messageReceiverService) =>
				{
					await messageReceiverService.ReceiveMessage(userID, chatID, message, context);
				});

			//app.Map(new PathString("/test/{data}"), (IApplicationBuilder app) =>
			//{
			//	app.UseMiddleware<MessageListPublisherService>();
			//});

			#region test

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

			app.UseAuthorization();

			app.MapRazorPages();

			app.Services.GetService<InitializeService>()?.Initialize();

			var disposeService = app.Services.GetService<DisposeService>();

			app.Run();

			disposeService?.Dispose();
		}
	}
}
