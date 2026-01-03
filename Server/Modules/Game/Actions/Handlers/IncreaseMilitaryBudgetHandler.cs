using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.IncreaseMilitaryBudget)]
public class IncreaseMilitaryBudgetHandler : IPlayerActionHandler
{
    public PlayerActionType Type => PlayerActionType.IncreaseMilitaryBudget;
    public Type? PayloadType => null;

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        _ = context;
        var playerCountry = gameState.GetPlayerCountry();
        playerCountry.Resources.Treasury -= 10;
        playerCountry.Resources.Manpower += 20;
        return Task.CompletedTask;
    }
}
