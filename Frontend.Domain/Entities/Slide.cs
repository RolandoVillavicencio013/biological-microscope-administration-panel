using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Entities;

/// <summary>Physical microscope slide. Belongs to a <see cref="Box"/> and can have multiple <see cref="Image"/> records.</summary>
public class Slide
{
    public int Id { get; set; }
    public SlideCode Code { get; set; } = null!;
    public SlideName Name { get; set; } = null!;
    public Text? Description { get; set; }
    public Text? Observations { get; set; }
    public int BoxId { get; set; }
    public Box Box { get; set; } = null!;
    public List<Image> Images { get; set; } = [];
}
