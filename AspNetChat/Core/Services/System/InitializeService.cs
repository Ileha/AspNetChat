using AspNetChat.Core.Interfaces.System;

namespace AspNetChat.Core.Services.System
{
	public class InitializeService : IInitializable
	{
		private readonly IEnumerable<IInitializable> _initializables;

		public InitializeService(IEnumerable<IInitializable> initializables)
		{
			_initializables = initializables ?? throw new ArgumentNullException(nameof(initializables));
		}

		public void Initialize()
		{
			foreach (var item in _initializables)
			{
				item.Initialize();
			}
		}
	}
}
