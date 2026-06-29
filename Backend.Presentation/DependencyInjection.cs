using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Endpoints;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation;

/// <summary>Provides extension methods for configuring dependency injection in the presentation layer.</summary>
public static class DependencyInjection
{
    /// <summary>Registers presentation layer services into the dependency injection container.</summary>
    public static IServiceCollection AddPresentationLayerServices(this IServiceCollection services)
    {
        return services;
    }

    /// <summary>Maps all presentation layer endpoints.</summary>
    public static IEndpointRouteBuilder MapPresentationLayerEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGroupEndpoints();
        builder.MapBoxEndpoints();
        builder.MapSlideEndpoints();
        builder.MapImageEndpoints();
        return builder;
    }
}
