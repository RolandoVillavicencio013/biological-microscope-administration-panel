using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles GET /slides — returns a page of slides, optionally filtered by code/name and/or box.</summary>
public static class ListSlidesHandler
{
    private const int MaxPageSize = 100;

    public static async Task<Results<Ok<PagedSlidesResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] ISlideService service,
        [FromQuery] string? search = null,
        [FromQuery] int? boxId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var errors = new List<ValidationError>();
        if (page < 1) errors.Add(new("Page", "Page must be at least 1."));
        if (pageSize is < 1 or > MaxPageSize) errors.Add(new("PageSize", $"PageSize must be between 1 and {MaxPageSize}."));
        if (errors.Count > 0) return TypedResults.BadRequest(new ErrorResponse(errors));

        var (items, totalCount) = await service.SearchAsync(search, boxId, page, pageSize);
        var response = new PagedSlidesResponse(items.Select(SlideMapper.ToResponse).ToList(), totalCount, page, pageSize);
        return TypedResults.Ok(response);
    }
}
