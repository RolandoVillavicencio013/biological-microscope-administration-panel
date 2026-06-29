using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Endpoints;

/// <summary>Maps all endpoints related to images.</summary>
public static class ImageEndpoints
{
    public static IEndpointRouteBuilder MapImageEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/slides/{id}/images", UploadImageHandler.HandleAsync)
            .DisableAntiforgery();
        builder.MapGet("/slides/{id}/images", ListImagesBySlideHandler.HandleAsync);
        builder.MapPut("/images/{id}", UpdateImageOrderHandler.HandleAsync);
        builder.MapDelete("/images/{id}", DeleteImageHandler.HandleAsync);
        return builder;
    }
}
