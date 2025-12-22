using System.Text.Json.Serialization;

namespace Server.Common.Llm.Dtos;

/// <summary>
/// LLM API response.
/// </summary>
/// <param name="Id">ID of the response.</param>
/// <param name="Provider">Provider of the response.</param>
/// <param name="Model">Model of the response.</param>
/// <param name="ObjectType">Object type of the response.</param>
/// <param name="Created">Created time of the response.</param>
/// <param name="Choices">Choices of the response.</param>
/// <param name="SystemFingerprint">System fingerprint of the response.</param>
/// <param name="Usage">Usage of the response.</param>
public record LlmApiResponse(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("provider")]
    string Provider,
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("object")] string ObjectType,
    [property: JsonPropertyName("created")]
    long Created,
    [property: JsonPropertyName("choices")]
    List<LlmApiChoice> Choices,
    [property: JsonPropertyName("system_fingerprint")]
    string SystemFingerprint,
    [property: JsonPropertyName("usage")] LlmApiUsage Usage
);