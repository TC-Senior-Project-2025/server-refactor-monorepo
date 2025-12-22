using System.Text.Json.Serialization;

namespace Server.Common.Llm.Dtos;

/// <summary>
/// LLM API usage.
/// </summary>
/// <param name="PromptTokens">Prompt tokens of the usage.</param>
/// <param name="CompletionTokens">Completion tokens of the usage.</param>
/// <param name="TotalTokens">Total tokens of the usage.</param>
/// <param name="PromptLlmTokensDetails">Prompt LLM tokens details of the usage.</param>
/// <param name="CompletionLlmTokensDetails">Completion LLM tokens details of the usage.</param>
public record LlmApiUsage(
    [property: JsonPropertyName("prompt_tokens")]
    int PromptTokens,
    [property: JsonPropertyName("completion_tokens")]
    int CompletionTokens,
    [property: JsonPropertyName("total_tokens")]
    int TotalTokens,
    [property: JsonPropertyName("prompt_tokens_details")]
    LlmTokenDetails PromptLlmTokensDetails,
    [property: JsonPropertyName("completion_tokens_details")]
    LlmTokenDetails CompletionLlmTokensDetails
);