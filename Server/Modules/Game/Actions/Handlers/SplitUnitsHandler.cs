using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.SplitUnit, typeof(SplitUnitDto))]
public class SplitUnitsHandler : IPlayerActionHandler
{
    public PlayerActionType Type => PlayerActionType.SplitUnit;
    public Type? PayloadType => typeof(SplitUnitDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        _ = context;
        var dto = payload as SplitUnitDto ?? throw new InvalidOperationException("SplitUnit payload is missing");

        var sourceUnit = gameState.Units.Find(u => u.Id == dto.UnitId);

        if (sourceUnit == null)
        {
            throw new InvalidOperationException($"Source unit {dto.UnitId} does not exist");
        }

        // Calculate split amounts
        int splitSize = sourceUnit.Size / 2;
        int splitSupply = sourceUnit.Supply / 2;

        if (splitSize <= 0)
        {
            // Optional: prevent splitting if unit is too small, 
            // but requirements just say "halves the selected unit"
            // If size is 1, splitSize is 0. 
            // We'll proceed but new unit might be 0 size if strict logic, 
            // but let's assume size is integer count of troops.
        }

        // Update source unit
        sourceUnit.Size -= splitSize;
        sourceUnit.Supply -= splitSupply;

        // Generate new ID
        int newId = (gameState.Units.Count > 0 ? gameState.Units.Max(u => u.Id) : 0) + 1;

        var newUnit = new Unit
        {
            Id = newId,
            CountryId = sourceUnit.CountryId,
            CommanderId = sourceUnit.CommanderId,
            LocationId = sourceUnit.LocationId,
            Name = $"{sourceUnit.Name} (Split)",
            Size = splitSize,
            Morale = sourceUnit.Morale,
            Supply = splitSupply,
            History = sourceUnit.History + $"\nSplit from unit {sourceUnit.Id} on turn {gameState.Turn}.",
            CanMove = sourceUnit.CanMove
        };

        gameState.Units.Add(newUnit);

        return Task.CompletedTask;
    }
}
