namespace Server.Modules.Game.Actions.Dto;

public class MoveUnitsDto
{
    public required Dictionary<int, int> UnitMovements { get; set; }
}