using Common.Extensions.DI.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions.DI;

public abstract class ServiceCollectionInstallerBase : IInstaller
{
    protected readonly IServiceCollection Services;

    protected ServiceCollectionInstallerBase(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public abstract void Install();
}