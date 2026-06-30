using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain;
using UCR.EB.BioMicroscopeAdmin.Backend.Application;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation;

namespace UCR.EB.BioMicroscopeAdmin.Backend.DependencyInjection;

/// <summary>Orchestrates dependency injection registration across all layers.</summary>
public static class DependencyInjection
{
    /// <summary>Registers all layer services into the dependency injection container.</summary>
    public static IServiceCollection AddCleanArchitectureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainLayerServices();
        services.AddApplicationLayerServices();
        services.AddInfrastructureLayerServices(configuration);
        services.AddPresentationLayerServices();
        return services;
    }

    /// <summary>Maps all presentation layer endpoints.</summary>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder) =>
        builder.MapPresentationLayerEndpoints();

    /// <summary>Handles unhandled exceptions and returns a JSON error response.</summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(x =>
        {
            x.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var ex = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>()?.Error;
                context.Response.StatusCode = ex switch
                {
                    Microsoft.AspNetCore.Http.BadHttpRequestException => 400,
                    System.Text.Json.JsonException => 400,
                    _ => 500
                };
                await context.Response.WriteAsJsonAsync(new
                {
                    error = context.Response.StatusCode == 400
                        ? "Malformed request."
                        : "An unexpected error occurred.",
                    details = ex?.Message
                });
            });
        });
        return app;
    }
}
