namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Provides a fluent interface for building event prompts with structured sections such as rules, context,
/// constraints, and output. The builder ensures a systemic composition of prompts with validation
/// and formatting utilities.
/// </summary>
public sealed class PromptBuilder
{
    private readonly List<string> _lines = [];

    private PromptBuilder() { }

    /// <summary>
    /// Creates a new prompt builder.
    /// </summary>
    /// <returns></returns>
    public static PromptBuilder Create() => new();

    internal void Add(string line)
    {
        if (!string.IsNullOrWhiteSpace(line))
            _lines.Add(line.Trim());
    }

    /// <summary>
    /// Builds the prompt string.
    /// </summary>
    /// <returns></returns>
    public string Build()
        => string.Join("\n", _lines);
}