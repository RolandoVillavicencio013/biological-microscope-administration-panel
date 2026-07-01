namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Boxes;

public record BoxDto(int Id, int Number, string Name, int GroupId);
public record CreateBoxDto(int Number, string Name, int GroupId);
public record UpdateBoxDto(int Number, string Name);
