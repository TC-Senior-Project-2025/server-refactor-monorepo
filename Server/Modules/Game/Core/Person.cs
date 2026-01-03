namespace Server.Modules.Game.Core;

public class Person
{
    public required int Id { get; set; }
    public required int CountryId { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required bool IsAlive { get; set; }
    public required int Loyalty { get; set; }
    public required string Role { get; set; }
    public required string History { get; set; }
    // public Stats Stats { get; set; }
}