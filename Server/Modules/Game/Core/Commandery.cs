namespace Server.Modules.Game.Core;

public class Commandery
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required int Population { get; set; }
}