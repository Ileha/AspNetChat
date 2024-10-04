namespace AspNetChat.Extensions
{
	public static class DIExtensions
	{
		public static void AddSingleton<TService1, ITService2, TInstance>(this IServiceCollection services)
			where TInstance : class, TService1, ITService2
			where TService1 : class
			where ITService2 : class
		{
			services.AddSingleton<TInstance>();

			services.AddSingleton<TService1, TInstance>(x => x.GetService<TInstance>());
			services.AddSingleton<ITService2, TInstance>(x => x.GetService<TInstance>());
		}

		public static void BindSingletonInterfacesTo<TInstance>(this IServiceCollection services)
			where TInstance : class
		{
			services.AddSingleton<TInstance>();

			var type = typeof(TInstance);

			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => p.IsInterface && type.IsAssignableFrom(p));

			foreach (var targetType in types)
			{
				services.AddSingleton(targetType, x => x.GetService<TInstance>());
			}

		}
	}
}
