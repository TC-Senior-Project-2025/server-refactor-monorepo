using Server.Modules.Assets;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;
using Microsoft.Extensions.Logging;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.MoveUnits, typeof(MoveUnitsDto))]
public class MoveUnitsHandler(AssetsService assetsService, ILogger<MoveUnitsHandler> logger) : IPlayerActionHandler
{
    private readonly AssetsService _assetsService = assetsService;
    private readonly ILogger<MoveUnitsHandler> _logger = logger;

    public PlayerActionType Type => PlayerActionType.MoveUnits;
    public Type? PayloadType => typeof(MoveUnitsDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        _ = context;
        var dto = payload as MoveUnitsDto ?? throw new InvalidOperationException("MoveUnits payload is missing");
        if (dto.UnitMovements == null)
        {
            throw new InvalidOperationException("No unit movements provided");
        }

        var connections = _assetsService.LoadMapConnections();
        if (connections == null)
        {
            _logger.LogError("Failed to load map connections");
            throw new InvalidOperationException("Failed to load map connections");
        }

        List<(Unit unit, int target)> unitsToMove = [];

        foreach (var (unitId, locationId) in dto.UnitMovements)
        {
            var unit = gameState.Units.Find(u => u.Id == unitId);

            if (unit == null)
            {
                throw new InvalidOperationException($"Unit {unitId} does not exist");
            }

            if (!gameState.Commanderies.ContainsKey(locationId))
            {
                throw new InvalidOperationException($"Commandery {locationId} does not exist");
            }

            if (!connections.IsNeighborOf(unit.LocationId, locationId))
            {
                throw new InvalidOperationException($"{unit.LocationId} is not a neighbor of {locationId}");
            }

            unitsToMove.Add((unit, locationId));
        }

        foreach (var pair in unitsToMove)
        {
            var (unit, targetLocationId) = pair;
            unit.LocationId = targetLocationId;
        }

        return Task.CompletedTask;
    }
}
