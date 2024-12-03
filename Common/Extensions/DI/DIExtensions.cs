using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions.DI;

public static partial class DIExtensions
{
    internal class ContainerWrapper : IServiceProvider
    {
        private readonly IComponentContext _context;

        public ContainerWrapper(IComponentContext context)
        {
            _context = context;
        }

        public object? GetService(Type serviceType)
        {
            // The method must return null if the service is not registered, otherwise, 'ActivatorUtilities.CreateInstance' will throw an exception
            _context.TryResolve(serviceType, out var service);
            return service;
        }
    }

    public static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters)
        where T : class
    {
        return ActivatorUtilities.CreateInstance<T>(provider, parameters);
    }

    public static void AddFactoryFromResolve<T>(this IServiceCollection services)
    {
        services.AddSingleton<IFactory<T>, FromResolveFactory<T>>(x => new FromResolveFactory<T>(x));
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
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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