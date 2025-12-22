namespace Server.Common.Llm.Options;

/// <summary>
/// LLM options.
/// </summary>
public sealed class LlmOptions
{
    /// <summary>
    /// API key.
    /// </summary>
    public string ApiKey { get; set; } = null!;
    
    public string Provider { get; set; } = null!;
    
    public int Timeout { get; set; } = 15;
}