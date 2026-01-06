namespace Server.Modules.Game.Actions.Dto;

public class CreateUnitDto
{
    public required string Name { get; set; }
    public required int Manpower { get; set; }
    public required int LocationId { get; set; }
}