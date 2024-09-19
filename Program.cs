namespace AspNetChat
{

    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

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

			//app.MapGet("/routes",
			//	(IEnumerable<EndpointDataSource> endpointSources) => string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));

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
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
    }
}
