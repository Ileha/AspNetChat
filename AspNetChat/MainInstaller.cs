using AspNetChat.Core.Services;
using Autofac;
using Common.Extensions.DI;

namespace AspNetChat;

public class MainInstaller : AutofacInstallerBase
{
    public MainInstaller(ContainerBuilder container) 
        : base(container)
    {
    }

    public override void Install()
    {
        // Builder
        //     .RegisterType<DisposeService>()
        //     .SingleInstance();
        //
        // Builder
        //     .RegisterType<InitializeService>()
        //     .SingleInstance();

        // Builder
        //     .AddFactory<WebApplication, App>()
        //     .SingleInstance();
    }
}