namespace Server.Common.Llm.Errors;

/// <summary>
/// Represents an exception thrown when a maximum retry limit is exceeded
/// while attempting to get a valid response from an LLM.
/// </summary>
public class LlmMaxRetriesException : Exception
{
    /// <inheritdoc/>
    public LlmMaxRetriesException(int retries, Exception? lastException) 
        : base($"Failed to get a valid response after {retries} retries.\nLast exception: {lastException}") { }
}