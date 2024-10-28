using AspNetChat.Core.Services.System;
using Common.Interfaces;

namespace AspNetChat.Core.Services;

public class App : IInitializable, IDisposable
{
    private readonly WebApplication _webApplication;
    private readonly DisposeService _disposeService;
    private readonly InitializeService _initializeService;

    public App(WebApplication webApplication, DisposeService disposeService, InitializeService initializeService)
    {
        _webApplication = webApplication ?? throw new ArgumentNullException(nameof(webApplication));
        _disposeService = disposeService ?? throw new ArgumentNullException(nameof(disposeService));
        _initializeService = initializeService ?? throw new ArgumentNullException(nameof(initializeService));
    }
    
    public void Initialize()
    {
        _initializeService.Initialize();
    }

    public void Run()
    {
        _webApplication.Run();
    }

    public void Dispose()
    {
        _disposeService.Dispose();
    }
}