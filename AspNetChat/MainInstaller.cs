using AspNetChat.Core.Services;
using AspNetChat.Core.Services.System;
using Common.Extensions.DI;

namespace AspNetChat;

public class MainInstaller : InstallerBase
{
    public MainInstaller(IServiceCollection services) 
        : base(services)
    {
    }

    public override void Install()
    {
        Services.AddSingleton<DisposeService>();
        Services.AddSingleton<InitializeService>();
        
        Services.AddFactory<WebApplication, App>();
    }
}