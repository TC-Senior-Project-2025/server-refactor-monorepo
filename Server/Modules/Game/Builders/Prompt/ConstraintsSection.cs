namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Represents a section within the event prompt builder that allows the configuration of constraints
/// for events, ensuring that specific rules and limitations are applied during prompt construction.
/// This section provides methods to define constraints such as resource delta limits, maximum options,
/// and the requirement of tradeoffs.
/// </summary>
public sealed class ConstraintsSection : PromptSection<PromptBuilder>
{
    /// <summary>
    /// Creates a constraints section.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="p"></param>
    public ConstraintsSection(PromptBuilder b, PromptBuilder p) : base(b, p)
    {
        Builder.Add("CONSTRAINTS:");
    }

    /// <summary>
    /// Defines a resource delta limit for the event.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public ConstraintsSection ResourceDeltaLimit(int min, int max)
    {
        Builder.Add($"- Resource delta per effect in [{min},{max}].");
        return this;
    }

    /// <summary>
    /// Defines a maximum number of options for the event.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public ConstraintsSection MaxOptions(int count)
    {
        Builder.Add($"- Max {count} options.");
        return this;
    }

    /// <summary>
    /// Requires that at least one option in the event have a tradeoff.
    /// </summary>
    /// <returns></returns>
    public ConstraintsSection RequireTradeoff()
    {
        Builder.Add("- At least one option must have a tradeoff.");
        return this;
    }
    
    /// <summary>
    /// Custom constraints section.
    /// </summary>
    /// <returns></returns>
    public ConstraintsSection Bullet(string custom)
    {
        Builder.Add($"- {custom}");
        return this;
    }
}
