using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions;

public interface IPlayerActionHandler
{
    PlayerActionType Type { get; }
    Type? PayloadType { get; }
    Task HandleAsync(GameActionContext context, GameState gameState, object? payload);
}
