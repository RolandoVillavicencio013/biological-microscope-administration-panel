using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Entities;

/// <summary>Image associated with a <see cref="Slide"/>. Has a display position within the slide (order 1-6).</summary>
public class Image
{
    public int Id { get; set; }
    public ImageUrl FileUrl { get; set; } = null!;
    public ImageFileName FileName { get; set; } = null!;
    public ImageOrder Order { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
    public int SlideId { get; set; }
    public Slide Slide { get; set; } = null!;
}
