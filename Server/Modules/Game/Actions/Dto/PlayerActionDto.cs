namespace Server.Modules.Game.Actions.Dto;

public class PlayerActionDto
{
    public required string Action { get; set; }
    public object? Payload { get; set; }
}