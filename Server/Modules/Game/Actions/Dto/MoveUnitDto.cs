namespace Server.Modules.Game.Actions.Dto;

public class MoveUnitDto
{
    public required int UnitId { get; init; }
    public required int LocationId { get; init; }
}