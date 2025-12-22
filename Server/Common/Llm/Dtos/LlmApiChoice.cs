using System.Text.Json.Serialization;

namespace Server.Common.Llm.Dtos;

/// <summary>
/// LLM API choice.
/// </summary>
/// <param name="Logprobs">Logprobs of the choice.</param>
/// <param name="FinishReason">Finish reason of the choice.</param>
/// <param name="NativeFinishReason">Native finish reason of the choice.</param>
/// <param name="Index">Index of the choice.</param>
/// <param name="Message">Message of the choice.</param>
public record LlmApiChoice(
    [property: JsonPropertyName("logprobs")]
    object Logprobs,

    [property: JsonPropertyName("finish_reason")]
    string FinishReason,

    [property: JsonPropertyName("native_finish_reason")]
    string NativeFinishReason,

    [property: JsonPropertyName("index")] int Index,

    [property: JsonPropertyName("message")]
    LlmApiMessage Message
);