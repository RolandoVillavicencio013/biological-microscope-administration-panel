using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

/// <summary>Physical box that groups microscope slides. Belongs to a <see cref="Group"/>.</summary>
public class Box
{
    /// <summary>Unique auto-incremental identifier.</summary>
    public int Id { get; }

    /// <summary>Physical number of the box.</summary>
    public BoxNumber Number { get; }

    /// <summary>Descriptive name of the box (e.g. "Cyanobacteria").</summary>
    public BoxName Name { get; }

    /// <summary>Foreign key to the biological group this box belongs to.</summary>
    public int GroupId { get; }

    /// <summary>Biological group this box belongs to. Populated by EF Core via Include.</summary>
    public Group Group { get; private set; } = null!;

    /// <summary>Slides contained in this box.</summary>
    public ICollection<Slide> Slides { get; } = [];

    /// <summary>Constructor for reconstruction from the database.</summary>
    public Box(int id, BoxNumber number, BoxName name, int groupId)
    {
        Id = id;
        Number = number;
        Name = name;
        GroupId = groupId;
    }

    /// <summary>Constructor for creating a new box (no id; assigned by the DB).</summary>
    public Box(BoxNumber number, BoxName name, int groupId)
    {
        Number = number;
        Name = name;
        GroupId = groupId;
    }
}
