using AspNetChat.Core.Interfaces.Factories;

namespace AspNetChat.Extensions.DI
{
    public static partial class DIExtensions
    {
        public static void AddFactoryTo<TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<TContract>, ResolveFactoryContract<TContract, TInstance>>(x => new ResolveFactoryContract<TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<TContract, TInstance> : IFactory<TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create()
			{
                return _serviceProvider.ResolveWith<TInstance>();
			}
		}

        public static void AddFactoryTo<P0, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, TContract>, ResolveFactoryContract<P0, TContract, TInstance>>(x => new ResolveFactoryContract<P0, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, TContract, TInstance> : IFactory<P0, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0 });
			}
		}

        public static void AddFactoryTo<P0, P1, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, TContract>, ResolveFactoryContract<P0, P1, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, TContract, TInstance> : IFactory<P0, P1, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, TContract>, ResolveFactoryContract<P0, P1, P2, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, TContract, TInstance> : IFactory<P0, P1, P2, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, TContract>, ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, TContract, TInstance> : IFactory<P0, P1, P2, P3, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8 });
			}
		}

        public static void AddFactoryTo<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>(this IServiceCollection services)
            where TInstance : class, TContract
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>, ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>>(x => new ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance>(x));
        }

        private class ResolveFactoryContract<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract, TInstance> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, TContract>
			where TInstance : class, TContract
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactoryContract(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public TContract Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9)
			{
                return _serviceProvider.ResolveWith<TInstance>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8, param9 });
			}
		}


        public static void AddFactory<T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<T>, ResolveFactory<T>>(x => new ResolveFactory<T>(x));
        }

        private class ResolveFactory<T> : IFactory<T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create()
			{
                return _serviceProvider.ResolveWith<T>();
			}
		}

        public static void AddFactory<P0, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, T>, ResolveFactory<P0, T>>(x => new ResolveFactory<P0, T>(x));
        }

        private class ResolveFactory<P0, T> : IFactory<P0, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0 });
			}
		}

        public static void AddFactory<P0, P1, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, T>, ResolveFactory<P0, P1, T>>(x => new ResolveFactory<P0, P1, T>(x));
        }

        private class ResolveFactory<P0, P1, T> : IFactory<P0, P1, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1 });
			}
		}

        public static void AddFactory<P0, P1, P2, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, T>, ResolveFactory<P0, P1, P2, T>>(x => new ResolveFactory<P0, P1, P2, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, T> : IFactory<P0, P1, P2, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, T>, ResolveFactory<P0, P1, P2, P3, T>>(x => new ResolveFactory<P0, P1, P2, P3, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, T> : IFactory<P0, P1, P2, P3, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, T>, ResolveFactory<P0, P1, P2, P3, P4, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, T> : IFactory<P0, P1, P2, P3, P4, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, P5, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, T>, ResolveFactory<P0, P1, P2, P3, P4, P5, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, P5, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, P5, T> : IFactory<P0, P1, P2, P3, P4, P5, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4, param5 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, T>, ResolveFactory<P0, P1, P2, P3, P4, P5, P6, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, P5, P6, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, P5, P6, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>, ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>, ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8 });
			}
		}

        public static void AddFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>, ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>>(x => new ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>(x));
        }

        private class ResolveFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T> : IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>
			where T : class
		{
			private readonly IServiceProvider _serviceProvider;

			public ResolveFactory(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
			}

			public T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9)
			{
                return _serviceProvider.ResolveWith<T>(parameters: new object[] { param0, param1, param2, param3, param4, param5, param6, param7, param8, param9 });
			}
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