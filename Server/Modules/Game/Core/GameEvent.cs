namespace Server.Modules.Game.Core;

public class GameEvent
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public List<GameEventOption> Options { get; init; } = [];
}

public class GameEventOption
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public class GameEventOptionEffects
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required FlagChangeset FlagChangeset { get; init; } = new();
}

public class FlagChangeset
{
    public List<string> Add { get; init; } = [];
    public List<string> Remove { get; init; } = [];
}

public record RecentEvent
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string? ChosenOptionTitle { get; init; } 
    public string? ChosenOptionDescription { get; init; }
}