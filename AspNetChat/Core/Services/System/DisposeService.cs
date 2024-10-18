namespace AspNetChat.Core.Services.System
{
	public class DisposeService : IDisposable
    {
        private readonly IEnumerable<IDisposable> _disposables;

        public DisposeService(IEnumerable<IDisposable> disposables)
        {
            _disposables = disposables ?? throw new ArgumentNullException(nameof(disposables));
        }

        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }
    }
}
