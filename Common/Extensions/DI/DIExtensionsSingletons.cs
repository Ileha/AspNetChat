using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions.DI
{
    public static partial class DIExtensions
    {
        public static void AddSingleton<TService0, TService1, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TService5, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4, TService5
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService5), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TService5, TService6, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4, TService5, TService6
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService5), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService6), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService5), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService6), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService7), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService5), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService6), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService7), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService8), x => x.GetService<TInstance>()!);
        }

        public static void AddSingleton<TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TInstance>(this IServiceCollection services)
            where TInstance : class, TService0, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton(typeof(TService0), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService1), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService2), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService3), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService4), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService5), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService6), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService7), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService8), x => x.GetService<TInstance>()!);
            services.AddSingleton(typeof(TService9), x => x.GetService<TInstance>()!);
        }

    }
}