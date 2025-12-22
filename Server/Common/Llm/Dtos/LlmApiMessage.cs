using System.Text.Json.Serialization;

namespace Server.Common.Llm.Dtos;

/// <summary>
/// LLM API message.
/// </summary>
/// <param name="Role">Role of the message.</param>
/// <param name="Content">Content of the message.</param>
/// <param name="Refusal">Refusal of the message.</param>
/// <param name="Reasoning">Reasoning of the message.</param>
public record LlmApiMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")]
    string Content,
    [property: JsonPropertyName("refusal")]
    object Refusal,
    [property: JsonPropertyName("reasoning")]
    object Reasoning
);