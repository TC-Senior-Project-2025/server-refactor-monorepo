using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.MergeUnits, typeof(MergeUnitsDto))]
public class MergeUnitsHandler : IPlayerActionHandler
{
    public PlayerActionType Type => PlayerActionType.MergeUnits;
    public Type? PayloadType => typeof(MergeUnitsDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        _ = context;
        var dto = payload as MergeUnitsDto ?? throw new InvalidOperationException("MergeUnits payload is missing");

        var sourceUnit = gameState.Units.Find(u => u.Id == dto.SourceUnitId);
        var targetUnit = gameState.Units.Find(u => u.Id == dto.TargetUnitId);

        if (sourceUnit == null)
        {
            throw new InvalidOperationException($"Source unit {dto.SourceUnitId} does not exist");
        }

        if (targetUnit == null)
        {
            throw new InvalidOperationException($"Target unit {dto.TargetUnitId} does not exist");
        }

        if (sourceUnit.LocationId != targetUnit.LocationId)
        {
            throw new InvalidOperationException($"Source unit {dto.SourceUnitId} and target unit {dto.TargetUnitId} are not in the same location");
        }

        targetUnit.Size += sourceUnit.Size;
        targetUnit.CanMove = sourceUnit.CanMove && targetUnit.CanMove;

        gameState.Units.Remove(sourceUnit);

        return Task.CompletedTask;
    }
}