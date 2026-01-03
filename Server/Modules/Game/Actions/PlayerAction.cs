using System.Text.Json;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;

namespace Server.Modules.Game.Actions;

public class PlayerAction
{
    public required PlayerActionType Action { get; init; }
    private JsonElement _payload;

    public static PlayerAction? From(PlayerActionDto dto)
    {
        if (Enum.TryParse(dto.Action, out PlayerActionType parsedAction))
        {
            var payloadElement = dto.Payload switch
            {
                JsonElement element => element,
                null => JsonSerializer.SerializeToElement<object?>(null),
                _ => JsonSerializer.SerializeToElement(dto.Payload)
            };

            return new PlayerAction()
            {
                Action = parsedAction,
                _payload = payloadElement
            };
        }
        return null;
    }

    public T? ParsePayload<T>()
    {
        if (_payload.ValueKind == JsonValueKind.Undefined || _payload.ValueKind == JsonValueKind.Null)
        {
            return default;
        }

        return _payload.Deserialize<T>();
    }

    public object? ParsePayload(Type type)
    {
        if (_payload.ValueKind == JsonValueKind.Undefined || _payload.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return JsonSerializer.Deserialize(_payload.GetRawText(), type);
    }
}
