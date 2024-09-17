namespace AspNetChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            //app.UseWelcomePage();
            //app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));

            app.UseWhen(
            context => context.Request.Path == "/time", // если путь запроса "/time"
            appBuilder =>
            {
                // логгируем данные - выводим на консоль приложения
                appBuilder.Use(async (context, next) =>
                {
                    var time = DateTime.Now.ToShortTimeString();
                    Console.WriteLine($"Time: {time}");
                    await next();   // вызываем следующий middleware
                });

                // отправляем ответ
                appBuilder.Run(async context =>
                {
                    var time = DateTime.Now.ToShortTimeString();
                    await context.Response.WriteAsync($"Time: {time}");
                });
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello METANIT.COM");
            });

            app.Run();
        }
    }
}
