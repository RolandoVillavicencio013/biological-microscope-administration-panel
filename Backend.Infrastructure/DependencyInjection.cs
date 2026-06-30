using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure;

/// <summary>Provides extension methods for configuring dependency injection in the infrastructure layer.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure layer services into the dependency injection container.
    /// Currently uses in-memory repositories — replace with SQL implementations when ready.
    /// </summary>
    public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services)
    {
        // Singleton so in-memory state persists for the lifetime of the application.
        services.AddSingleton<IGroupRepository, InMemoryGroupRepository>();
        services.AddSingleton<IBoxRepository, InMemoryBoxRepository>();
        services.AddSingleton<ISlideRepository, InMemorySlideRepository>();
        services.AddSingleton<IImageRepository, InMemoryImageRepository>();

        services.AddSingleton<IFileStorageService, LocalFileStorageService>();
        return services;
    }
}
