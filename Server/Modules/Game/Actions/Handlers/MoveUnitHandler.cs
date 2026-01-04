using Server.Modules.Assets;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.MoveUnit, typeof(MoveUnitDto))]
public class MoveUnitHandler(AssetsService assetsService, ILogger<MoveUnitHandler> logger) : IPlayerActionHandler
{
    private readonly AssetsService _assetsService = assetsService;
    private readonly ILogger<MoveUnitHandler> _logger = logger;

    public PlayerActionType Type => PlayerActionType.MoveUnit;
    public Type? PayloadType => typeof(MoveUnitDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        _ = context;
        var dto = payload as MoveUnitDto ?? throw new InvalidOperationException("MoveUnit payload is missing");
        var connections = _assetsService.LoadMapConnections();

        if (connections == null)
        {
            _logger.LogError("Failed to load map connections");
            throw new InvalidOperationException("Failed to load map connections");
        }

        var unitId = dto.UnitId;
        var locationId = dto.LocationId;

        var unit = gameState.Units.Find(u => u.Id == unitId)
            ?? throw new InvalidOperationException($"Unit {unitId} does not exist");

        if (!unit.CanMove)
        {
            throw new InvalidOperationException($"Cannot move unit");
        }

        if (!gameState.Commanderies.ContainsKey(locationId))
        {
            throw new InvalidOperationException($"Commandery {locationId} does not exist");
        }

        if (!connections.IsNeighborOf(unit.LocationId, locationId))
        {
            throw new InvalidOperationException($"{unit.LocationId} is not a neighbor of {locationId}");
        }

        unit.LocationId = locationId;
        unit.CanMove = false;

        return Task.CompletedTask;
    }
}