namespace Server.Modules.Game.Core;

public class Unit
{
    public required int Id { get; set; }

    public required int CountryId { get; set; }

    public required int? CommanderId { get; set; }

    public required int LocationId { get; set; }

    public required string Name { get; set; }

    public required int Size { get; set; }

    public required int Morale { get; set; }

    public required int Supply { get; set; }

    public required string History { get; set; }

    public bool CanMove { get; set; } = true;
}