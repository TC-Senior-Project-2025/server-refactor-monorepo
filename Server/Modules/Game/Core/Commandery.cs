namespace Server.Modules.Game.Core;

public class Commandery
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required int Population { get; set; }
    public required int Wealth { get; set; }
    public required int Unrest { get; set; }
    public required int CountryId { get; set; }
}