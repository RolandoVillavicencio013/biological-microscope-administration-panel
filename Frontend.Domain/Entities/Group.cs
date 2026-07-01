using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Entities;

/// <summary>Biological group that organizes boxes (e.g. "Bacteria").</summary>
public class Group
{
    public int Id { get; set; }
    public GroupName Name { get; set; } = null!;
    public List<Box> Boxes { get; set; } = [];
}
