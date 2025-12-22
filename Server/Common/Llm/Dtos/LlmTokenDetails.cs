using System.Text.Json.Serialization;

namespace Server.Common.Llm.Dtos;

/// <summary>
/// LLM token details.
/// </summary>
/// <param name="CachedTokens">Cached tokens of the details.</param>
/// <param name="AudioTokens">Audio tokens of the details.</param>
/// <param name="ReasoningTokens">Reasoning tokens of the details.</param>
public record LlmTokenDetails(
    [property: JsonPropertyName("cached_tokens")]
    int CachedTokens,
    [property: JsonPropertyName("audio_tokens")]
    int AudioTokens,
    [property: JsonPropertyName("reasoning_tokens")]
    int? ReasoningTokens
);