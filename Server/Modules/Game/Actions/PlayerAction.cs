using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;

namespace Server.Modules.Game.Actions;

public class PlayerAction
{
    public required PlayerActionType Action { get; init; }
    private object? _payload;

    public static PlayerAction? From(PlayerActionDto dto)
    {
        if (Enum.TryParse(dto.Action, out PlayerActionType parsedAction))
        {
            return new PlayerAction()
            {
                Action = parsedAction,
                _payload = dto.Payload
            };
        }
        return null;
    }

    public T? ParsePayload<T>()
    {
        return JsonSerializer.Deserialize<T>(_payload?.ToString() ?? "");
    }
}