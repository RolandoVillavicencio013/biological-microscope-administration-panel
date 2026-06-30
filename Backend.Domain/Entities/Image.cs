using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

/// <summary>Image associated with a <see cref="Slide"/>. Has a display position within the slide (order 1–6).</summary>
public class Image
{
    /// <summary>Unique auto-incremental identifier.</summary>
    public int Id { get; }

    /// <summary>File path or URL of the image.</summary>
    public ImageUrl FileUrl { get; }

    /// <summary>Original file name of the image.</summary>
    public ImageFileName FileName { get; }

    /// <summary>Display position within the slide (1–6). Mutable after creation (RF-3).</summary>
    public ImageOrder Order { get; set; } = null!;

    /// <summary>UTC date and time when the image was uploaded to the system.</summary>
    public DateTime UploadedAt { get; }

    /// <summary>Foreign key to the slide this image belongs to.</summary>
    public int SlideId { get; }

    /// <summary>Slide this image belongs to. Populated by EF Core via Include.</summary>
    public Slide Slide { get; private set; } = null!;

    /// <summary>Constructor for reconstruction from the database.</summary>
    public Image(int id, ImageUrl fileUrl, ImageFileName fileName, ImageOrder order,
        DateTime uploadedAt, int slideId)
    {
        Id = id;
        FileUrl = fileUrl;
        FileName = fileName;
        Order = order;
        UploadedAt = uploadedAt;
        SlideId = slideId;
    }

    /// <summary>Constructor for creating a new image (no id; assigned by the DB). Records the current UTC timestamp.</summary>
    public Image(ImageUrl fileUrl, ImageFileName fileName, ImageOrder order, int slideId)
    {
        FileUrl = fileUrl;
        FileName = fileName;
        Order = order;
        UploadedAt = DateTime.UtcNow;
        SlideId = slideId;
    }
}
