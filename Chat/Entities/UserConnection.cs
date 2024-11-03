using System.Net.WebSockets;
using Common.Interfaces;

namespace Chat.Entities;

public class UserConnection : IDisposable, IIdentifiable
{
    private readonly IReadOnlyList<IDisposable> _disposables;
    public WebSocket WebSocket { get; }
    public IIdentifiable Chat { get; }
    public IIdentifiable User { get; }
    public CancellationToken CancellationToken { get; }
    public Guid Id { get; }

    private readonly CancellationTokenSource _cancellationTokenSource;

    public UserConnection(
        Guid id,
        WebSocket webSocket, 
        IIdentifiable chat, 
        IIdentifiable user,
        CancellationToken token, 
        params IDisposable[] disposables)
    {
        _disposables = disposables ?? throw new ArgumentNullException(nameof(disposables));
        Id = id;
        WebSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
        Chat = chat ?? throw new ArgumentNullException(nameof(chat));
        User = user ?? throw new ArgumentNullException(nameof(user));
			
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        CancellationToken = _cancellationTokenSource.Token;
    }

    public void Dispose()
    {
        WebSocket.Dispose();
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();

        foreach (var disposable in _disposables) 
            disposable.Dispose();
    }
}