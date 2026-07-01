using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure;

/// <summary>Provides extension methods for configuring dependency injection in the infrastructure layer.</summary>
public static class DependencyInjection
{
    /// <summary>Registers infrastructure layer services into the dependency injection container.</summary>
    public static IServiceCollection AddInfrastructureLayerServices(
        this IServiceCollection services, string apiBaseUrl)
    {
        services.AddHttpClient<BioMicroscopeApiClient>(c =>
            c.BaseAddress = new Uri(apiBaseUrl));

        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IBoxRepository, BoxRepository>();
        services.AddScoped<ISlideRepository, SlideRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        return services;
    }
}
