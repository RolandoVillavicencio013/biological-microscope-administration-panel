using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Entities;

/// <summary>Physical box that groups microscope slides. Belongs to a <see cref="Group"/>.</summary>
public class Box
{
    public int Id { get; set; }
    public BoxNumber Number { get; set; } = null!;
    public BoxName Name { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public List<Slide> Slides { get; set; } = [];
}
