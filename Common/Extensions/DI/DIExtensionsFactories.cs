using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Builder;

namespace Common.Extensions.DI
{
    public static partial class DIExtensions
    {
        public static IRegistrationBuilder<
            IFactory<TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<TContract, TInstance>>()
                .As<IFactory<TContract>>();
        }
        
        private class AutofacResolveFactoryContract<TContract, TInstance> : IFactory<TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create()
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>();
            }
        }

        public static void AddFactoryTo<TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<TContract>, ResolveFactoryContract<TContract, TInstance>>(x => new ResolveFactoryContract<TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<TContract, TInstance> : IFactory<TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create()
			{
                return _serviceProvider.ResolveWith<TInstance>();
			}
		}

        public static IRegistrationBuilder<
            IFactory<TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<TInstance, TInstance>>()
                .As<IFactory<TInstance>>();
        }

        public static void AddFactory<T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<T>, ResolveFactoryContract<T, T>>(x => new ResolveFactoryContract<T, T>(x));
        }

        public static void AddFactoryFromMethod<T>(this IServiceCollection services, Func<IServiceProvider, T> func)
        {
            services.AddSingleton<IFactory<T>, FactoryFromMethod<T>>(x => new FactoryFromMethod<T>(x, func));
        }

        private class FactoryFromMethod<T> : IFactory<T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider,  T> _func;

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

        public static IRegistrationBuilder<
            IFactory<P0, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, TContract, TInstance>>()
                .As<IFactory<P0, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, TContract, TInstance> : IFactory<P0, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0 ]);
            }
        }

        public static void AddFactoryTo<P0, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, TContract>, ResolveFactoryContract<P0, TContract, TInstance>>(x => new ResolveFactoryContract<P0, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, TContract, TInstance> : IFactory<P0, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, TInstance, TInstance>>()
                .As<IFactory<P0, TInstance>>();
        }

        public static void AddFactory<P0, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, T>, ResolveFactoryContract<P0, T, T>>(x => new ResolveFactoryContract<P0, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, T>(this IServiceCollection services, Func<IServiceProvider, P0, T> func)
        {
            services.AddSingleton<IFactory<P0, T>, FactoryFromMethod<P0, T>>(x => new FactoryFromMethod<P0, T>(x, func));
        }

        private class FactoryFromMethod<P0, T> : IFactory<P0, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0)
            {
                return _func.Invoke(_serviceProvider, param0);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, TContract, TInstance>>()
                .As<IFactory<P0, P1, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, TContract, TInstance> : IFactory<P0, P1, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, TContract>, ResolveFactoryContract<P0, P1, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, TContract, TInstance> : IFactory<P0, P1, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, TInstance, TInstance>>()
                .As<IFactory<P0, P1, TInstance>>();
        }

        public static void AddFactory<P0, P1, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, T>, ResolveFactoryContract<P0, P1, T, T>>(x => new ResolveFactoryContract<P0, P1, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, T>, FactoryFromMethod<P0, P1, T>>(x => new FactoryFromMethod<P0, P1, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, T> : IFactory<P0, P1, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1)
            {
                return _func.Invoke(_serviceProvider, param0, param1);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, TContract, TInstance> : IFactory<P0, P1, P2, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, TContract>, ResolveFactoryContract<P0, P1, P2, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, TContract, TInstance> : IFactory<P0, P1, P2, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, T>, ResolveFactoryContract<P0, P1, P2, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, T>, FactoryFromMethod<P0, P1, P2, T>>(x => new FactoryFromMethod<P0, P1, P2, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, T> : IFactory<P0, P1, P2, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance> : IFactory<P0, P1, P2, P3, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, TContract>, ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance> : IFactory<P0, P1, P2, P3, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, T>, ResolveFactoryContract<P0, P1, P2, P3, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, T>, FactoryFromMethod<P0, P1, P2, P3, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, T> : IFactory<P0, P1, P2, P3, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, T>, FactoryFromMethod<P0, P1, P2, P3, P4, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, T> : IFactory<P0, P1, P2, P3, P4, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, P5, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4, param5 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, P5, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, P5, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, P5, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, P5, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, T>, FactoryFromMethod<P0, P1, P2, P3, P4, P5, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, P5, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, P5, T> : IFactory<P0, P1, P2, P3, P4, P5, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4, P5,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, P5, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4, param5);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4, param5, param6 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, P5, P6, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, T>, FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4, param5, param6);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4, param5, param6, param7 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>, FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4, param5, param6, param7);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4, param5, param6, param7, param8 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>, FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4, param5, param6, param7, param8);
            }
        }

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class, TContract
            where TContract : notnull
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>>();
        }
        
        private class AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract> 
            where TInstance : class, TContract
            where TContract : notnull
        {
            private readonly ILifetimeScope _scope;

            public AutofacResolveFactoryContract(ILifetimeScope scope)
            {
                _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            }

            public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9)
            {
                return new ContainerWrapper(_scope).ResolveWith<TInstance>(parameters: [ param0, param1, param2, param3, param4, param5, param6, param7, param8, param9 ]);
            }
        }

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>(x));
        }

        public class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8, param9 });
			}
		}

        public static IRegistrationBuilder<
            IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TInstance>, 
            ConcreteReflectionActivatorData, 
            SingleRegistrationStyle> AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TInstance>(this ContainerBuilder typeSourceSelector)
            where TInstance : class
        {
            return typeSourceSelector
                .RegisterType<AutofacResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TInstance, TInstance>>()
                .As<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TInstance>>();
        }

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T, T>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T, T>(x));
        }

        public static void AddFactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>(this IServiceCollection services, Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T> func)
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>, FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>>(x => new FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>(x, func));
        }

        private class FactoryFromMethod<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8, P9,  T> _func;

            public FactoryFromMethod(
                IServiceProvider serviceProvider,
                Func<IServiceProvider, P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T> func)
            {
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _func = func ?? throw new ArgumentNullException(nameof(func));
            }

            public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9)
            {
                return _func.Invoke(_serviceProvider, param0, param1, param2, param3, param4, param5, param6, param7, param8, param9);
            }
        }

    }
}