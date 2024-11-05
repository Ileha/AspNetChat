using Autofac;
using Common.Extensions.DI.Interfaces;

namespace Common.Extensions.DI;

public abstract class AutofacInstallerBase : IInstaller
{
    protected readonly ContainerBuilder Builder;

    protected AutofacInstallerBase(ContainerBuilder builder)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public abstract void Install();
}