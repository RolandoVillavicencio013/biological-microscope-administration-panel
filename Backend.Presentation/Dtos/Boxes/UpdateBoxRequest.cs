namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;

/// <summary>Request body for updating a box's number and name.</summary>
public record UpdateBoxRequest(int Number, string Name);
