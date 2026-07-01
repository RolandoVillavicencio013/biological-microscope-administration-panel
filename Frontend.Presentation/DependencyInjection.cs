using Microsoft.Extensions.DependencyInjection;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Presentation;

/// <summary>Provides extension methods for configuring dependency injection in the presentation layer.</summary>
public static class DependencyInjection
{
    /// <summary>Registers presentation layer services into the dependency injection container.</summary>
    public static IServiceCollection AddPresentationLayerServices(this IServiceCollection services)
    {
        // No additional registrations needed; MudBlazor is added by the app entry point.
        return services;
    }
}
