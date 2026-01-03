using Server.Modules.Game.Actions.Enums;

namespace Server.Modules.Game.Actions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PlayerActionHandlerAttribute : Attribute
{
    public PlayerActionHandlerAttribute(PlayerActionType type, Type? payloadType = null)
    {
        Type = type;
        PayloadType = payloadType;
    }

    public PlayerActionType Type { get; }
    public Type? PayloadType { get; }
}
