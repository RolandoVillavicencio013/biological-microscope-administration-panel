using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure;
using UCR.EB.BioMicroscopeAdmin.Frontend.Presentation;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.DependencyInjection;

/// <summary>Orchestrates dependency injection registration across all frontend layers.</summary>
public static class DependencyInjection
{
    /// <summary>Registers all frontend layer services into the dependency injection container.</summary>
    public static IServiceCollection AddFrontendServices(
        this IServiceCollection services, string apiBaseUrl)
    {
        services.AddDomainLayerServices();
        services.AddApplicationLayerServices();
        services.AddInfrastructureLayerServices(apiBaseUrl);
        services.AddPresentationLayerServices();
        return services;
    }
}
