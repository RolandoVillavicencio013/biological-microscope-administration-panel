using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

/// <summary>Physical microscope slide. Belongs to a <see cref="Box"/> and can have multiple <see cref="Image"/> records.</summary>
public class Slide
{
    /// <summary>Unique auto-incremental identifier.</summary>
    public int Id { get; }

    /// <summary>Composite code of the slide (e.g. "1TB07").</summary>
    public SlideCode Code { get; }

    /// <summary>Descriptive name of the slide (e.g. "Gloecapsa").</summary>
    public SlideName Name { get; }

    /// <summary>Optional description of the slide. Mutable after creation (RF-3).</summary>
    public Text? Description { get; set; }

    /// <summary>Optional observations from the sample. Mutable after creation (RF-3).</summary>
    public Text? Observations { get; set; }

    /// <summary>Foreign key to the physical box containing this slide.</summary>
    public int BoxId { get; }

    /// <summary>Physical box containing this slide. Populated by EF Core via Include.</summary>
    public Box Box { get; private set; } = null!;

    /// <summary>Images associated with the slide (typically up to 6). Mutable after creation (RF-3).</summary>
    public ICollection<Image> Images { get; } = [];

    /// <summary>Constructor for reconstruction from the database.</summary>
    public Slide(int id, SlideCode code, SlideName name, int boxId,
        Text? description = null, Text? observations = null)
    {
        Id = id;
        Code = code;
        Name = name;
        BoxId = boxId;
        Description = description;
        Observations = observations;
    }

    /// <summary>Constructor for creating a new slide (no id; assigned by the DB).</summary>
    public Slide(SlideCode code, SlideName name, int boxId,
        Text? description = null, Text? observations = null)
    {
        Code = code;
        Name = name;
        BoxId = boxId;
        Description = description;
        Observations = observations;
    }
}
