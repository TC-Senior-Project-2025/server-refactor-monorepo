using Server.Modules.Game.Core;

namespace Server.Modules.Game.Dto;

public class StartGameResponse
{
    public required Dictionary<int, List<int>> MapConnections { get; init; }
    public required GameState GameState { get; init; }
}