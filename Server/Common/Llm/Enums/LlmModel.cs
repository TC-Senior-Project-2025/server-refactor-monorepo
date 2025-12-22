namespace Server.Common.Llm.Enums;

/// <summary>
/// LLM models.
/// </summary>
public enum LlmModel
{
    /// <summary>
    /// GPT-5.2 chat model.
    /// </summary>
    OpenRouterGpt52Chat
}

/// <summary>
/// Extension methods for <see cref="LlmModel"/>.
/// </summary>
public static class OpenRouterLlmModelExtensions
{
    /// <summary>
    /// Converts the specified OpenRouterLlmModel enum value to its corresponding string representation.
    /// </summary>
    /// <param name="model">The OpenRouterLlmModel enum value to convert.</param>
    /// <returns>The string representation of the specified model.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided model value is not supported.</exception>
    public static string AsString(this LlmModel model)
    {
        return model switch
        {
            LlmModel.OpenRouterGpt52Chat => "openai/gpt-5.2-chat",
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };        
    }
}