using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Endpoints;

/// <summary>Maps all endpoints related to boxes.</summary>
public static class BoxEndpoints
{
    public static IEndpointRouteBuilder MapBoxEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/boxes", CreateBoxHandler.HandleAsync);
        builder.MapGet("/boxes", ListBoxesHandler.HandleAsync);
        builder.MapGet("/boxes/{id}", GetBoxHandler.HandleAsync);
        builder.MapGet("/groups/{id}/boxes", ListBoxesByGroupHandler.HandleAsync);
        builder.MapPut("/boxes/{id}", UpdateBoxHandler.HandleAsync);
        builder.MapDelete("/boxes/{id}", DeleteBoxHandler.HandleAsync);
        return builder;
    }
}
