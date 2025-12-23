using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;

namespace Server.Modules.Game.Actions;

public class PlayerAction
{
    public required PlayerActionType Action { get; init; }

    public static PlayerAction? From(PlayerActionDto dto)
    {
        if (Enum.TryParse(dto.Action, out PlayerActionType parsedAction))
        {
            return new PlayerAction()
            {
                Action = parsedAction
            };
        }
        return null;
    }
}