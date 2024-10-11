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
	}
}
