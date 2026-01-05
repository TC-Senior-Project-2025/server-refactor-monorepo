namespace Server.Modules.Game.Actions.Dto;

public class ResupplyUnitDto
{
    public required int UnitId { get; set; }
    public required int Amount { get; set; }
}