using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Endpoints;

/// <summary>Maps all endpoints related to slides.</summary>
public static class SlideEndpoints
{
    public static IEndpointRouteBuilder MapSlideEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/slides", CreateSlideHandler.HandleAsync);
        builder.MapGet("/slides", ListSlidesHandler.HandleAsync);
        builder.MapGet("/slides/{id}", GetSlideHandler.HandleAsync);
        builder.MapGet("/boxes/{id}/slides", ListSlidesByBoxHandler.HandleAsync);
        builder.MapPut("/slides/{id}", UpdateSlideHandler.HandleAsync);
        builder.MapDelete("/slides/{id}", DeleteSlideHandler.HandleAsync);
        return builder;
    }
}
