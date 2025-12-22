using Server.Common.Llm.Dtos;
using Server.Common.Llm.Enums;

namespace Server.Common.Llm.Interfaces;

/// <summary>
/// Interface for LLM service.
/// </summary>
public interface ILlmService
{
    /// <summary>
    /// Asks the LLM for a text-based response using the provided prompt.
    /// </summary>
    /// <param name="prompt">The input prompt to guide the LLM's response.</param>
    /// <param name="model">The LLM model to use for generating the response.</param>
    /// <param name="maxRetries">The maximum number of retry attempts in case of failure. Default is 3.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the LLM.</returns>
    public Task<LlmApiResponse> AskText(string prompt, LlmModel model = LlmModel.OpenRouterGpt52Chat, int maxRetries = 3);
}