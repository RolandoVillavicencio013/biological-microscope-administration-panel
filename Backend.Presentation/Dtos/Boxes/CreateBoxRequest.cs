namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;

/// <summary>Request body for creating a new box.</summary>
public record CreateBoxRequest(int Number, string Name, int GroupId);
