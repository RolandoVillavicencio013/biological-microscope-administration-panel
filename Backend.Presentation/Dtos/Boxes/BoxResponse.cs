namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;

/// <summary>Response body representing a box.</summary>
public record BoxResponse(int Id, int Number, string Name, int GroupId);
