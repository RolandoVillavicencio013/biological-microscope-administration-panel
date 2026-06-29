using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

/// <summary>Biological group that organizes boxes (e.g. "Bacteria").</summary>
public class Group
{
    /// <summary>Unique auto-incremental identifier.</summary>
    public int Id { get; }

    /// <summary>Name of the biological group.</summary>
    public GroupName Name { get; }

    /// <summary>Physical boxes belonging to this group.</summary>
    public ICollection<Box> Boxes { get; } = [];

    /// <summary>Constructor for reconstruction from the database.</summary>
    public Group(int id, GroupName name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>Constructor for creating a new group (no id; assigned by the DB).</summary>
    public Group(GroupName name)
    {
        Name = name;
    }
}
