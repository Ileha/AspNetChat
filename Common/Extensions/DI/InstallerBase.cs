using Common.Extensions.DI.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions.DI;

public abstract class InstallerBase : IInstaller
{
    protected readonly IServiceCollection Services;

    protected InstallerBase(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public abstract void Install();
}