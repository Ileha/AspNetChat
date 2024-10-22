using AspNetChat.Extensions.DI;

namespace AspNetChat.Extensions.DI
{
	public static partial class DIExtensions
    {
        public static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters)
            where T : class
        {
            return ActivatorUtilities.CreateInstance<T>(provider, parameters);
        }

        public static void AddFactoryFromResolve<T>(this IServiceCollection services)
        {
            services.AddSingleton<IFactory<T>, FromResolveFactory<T>>(x => new FromResolveFactory<T>(x));
        }

		public static void AddSingleton<TService1, ITService2, TInstance>(this IServiceCollection services)
            where TInstance : class, TService1, ITService2
            where TService1 : class
            where ITService2 : class
        {
            services.AddSingleton<TInstance>();

            services.AddSingleton<TService1, TInstance>(x => x.GetService<TInstance>()!);
            services.AddSingleton<ITService2, TInstance>(x => x.GetService<TInstance>()!);
        }

        public static void BindSingletonInterfacesTo<TInstance>(this IServiceCollection services)
            where TInstance : class
        {
            services.AddSingleton<TInstance>();

            var type = typeof(TInstance);

            foreach (var targetType in type.GetInterfaces())
            {
                services.AddSingleton(targetType, x => x.GetService<TInstance>()!);
            }
        }

        public static void BindSingletonInterfacesTo<TInstance>(this IServiceCollection services, params object[] parameters)
            where TInstance : class
        {
            services.AddSingleton(p => p.ResolveWith<TInstance>(parameters));

            var type = typeof(TInstance);

            foreach (var targetType in type.GetInterfaces())
            {
                services.AddSingleton(targetType, x => x.GetService<TInstance>()!);
            }
        }

		private class FromResolveFactory<T> : IFactory<T>
        {
            private readonly IServiceProvider _serviceProvider;
            private T? _instance = default;

            public FromResolveFactory(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
            }

            public T Create()
            {
                _instance ??= _serviceProvider.GetService<T>();

                if (_instance == null)
                    throw new InvalidOperationException($"can't find service type of {typeof(T)}");

                return _instance;
            }
        }
    }
}
