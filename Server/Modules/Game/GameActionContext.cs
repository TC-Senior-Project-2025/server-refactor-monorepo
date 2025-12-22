using System.Net.WebSockets;
using System.Text.Json;
using Server.Modules.Users.Entities;

namespace Server.Modules.Game;

/// <summary>
/// Encapsulates the core data elements required when executing a specific game-related operation.
/// This includes the parsed payload of an action, the WebSocket associated with the client session,
/// and the user initiating the action. It serves as an input parameter across various game action
/// handlers to simplify context management and ensure data consistency during operation execution.
/// </summary>
public class GameActionContext
{
    /// <summary>
    /// Represents the payload data associated with a game action. This property contains
    /// the serialized JSON data encapsulating relevant information required to process specific
    /// game-related operations. The payload is typically extracted and parsed to retrieve
    /// details like game state, user actions, or configurations.
    /// </summary>
    public required JsonElement Payload { get; init; }

    /// <summary>
    /// Represents the WebSocket connection associated with a game action context.
    /// This property is used for sending and receiving real-time messages between the server and a connected client.
    /// The socket facilitates bidirectional communication and allows the server to push updates
    /// and receive data related to game operations from the client.
    /// </summary>
    public required WebSocket Socket { get; init; }

    /// <summary>
    /// Represents the user initiating the game action. This property provides access
    /// to the core user data, including identification and authentication details,
    /// such as the user ID and username. It is used to determine the context and
    /// permissions of the user for the execution of specific game-related operations.
    /// </summary>
    public required UserEntity UserEntity { get; init; }
}