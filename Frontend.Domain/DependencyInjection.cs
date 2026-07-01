using Microsoft.Extensions.DependencyInjection;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain;

/// <summary>Provides extension methods for configuring dependency injection in the domain layer.</summary>
public static class DependencyInjection
{
    /// <summary>Registers domain layer services into the dependency injection container.</summary>
    public static IServiceCollection AddDomainLayerServices(this IServiceCollection services)
    {
        return services;
    }
}
