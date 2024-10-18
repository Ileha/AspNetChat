using AspNetChat.Core.Interfaces.Factories;

namespace AspNetChat.Extensions
{
	public static class DIExtensions
	{
		public static void AddFactoryFromResolve<T>(this IServiceCollection services) 
		{
			services.AddSingleton<IFactory<T>, ResolveFactory<T>>(x => new ResolveFactory<T>(x));
		}

		public static void AddFactoryFromMethod<T>(this IServiceCollection services, Func<IServiceProvider, T> func) 
		{
			services.AddSingleton<IFactory<T>, FactoryFromMethod<T>>(x => new FactoryFromMethod<T>(x, func));
		}

		public static void AddFactoryFromMethod<P1, T>(this IServiceCollection services, Func<IServiceProvider, P1, T> func) 
		{
			services.AddSingleton<IFactory<P1, T>, FactoryFromMethod<P1, T>>(x => new FactoryFromMethod<P1, T>(x, func));
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

		private class FactoryFromMethod<T> : IFactory<T>
		{
			private readonly IServiceProvider _serviceProvider;
			private readonly Func<IServiceProvider, T> _func;

			public FactoryFromMethod(
				IServiceProvider serviceProvider, 
				Func<IServiceProvider, T> func) 
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
				_func = func ?? throw new ArgumentNullException(nameof(func));
			}

			public T Create()
			{
				return _func.Invoke(_serviceProvider);
			}
		}

		private class FactoryFromMethod<P1, T> : IFactory<P1, T>
		{
			private readonly IServiceProvider _serviceProvider;
			private readonly Func<IServiceProvider, P1, T> _func;

			public FactoryFromMethod(
				IServiceProvider serviceProvider, 
				Func<IServiceProvider, P1, T> func) 
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
				_func = func ?? throw new ArgumentNullException(nameof(func));
			}

			public T Create(P1 param1)
			{
				return _func.Invoke(_serviceProvider, param1);
			}
		}

		private class ResolveFactory<T> : IFactory<T>
		{
			private readonly IServiceProvider _serviceProvider;
			private T? _instance = default;

			public ResolveFactory(IServiceProvider serviceProvider)
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
