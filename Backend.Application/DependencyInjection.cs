using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application;

/// <summary>Provides extension methods for configuring dependency injection in the application layer.</summary>
public static class DependencyInjection
{
    /// <summary>Registers application layer services into the dependency injection container.</summary>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IBoxService, BoxService>();
        services.AddScoped<ISlideService, SlideService>();
        services.AddScoped<IImageService, ImageService>();
        return services;
    }
}
