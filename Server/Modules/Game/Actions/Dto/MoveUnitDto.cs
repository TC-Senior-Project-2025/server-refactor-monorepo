namespace Server.Modules.Game.Actions.Dto;

public class MoveUnitDto
{
    public required int UnitId { get; set; }
    public required int LocationId { get; set; }
}