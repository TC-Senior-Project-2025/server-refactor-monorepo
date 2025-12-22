namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Provides extension methods for enhancing the functionality of the <see cref="PromptBuilder"/> class,
/// enabling the addition of structured sections such as system messages, rules, context, constraints, and output.
/// These methods allow for streamlined and fluent composition of event prompts.
/// </summary>
public static class PromptBuilderExtensions
{
    /// <summary>
    /// Adds a system message to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static PromptBuilder AsSystem(this PromptBuilder pb, string text)
    {
        pb.Add("SYSTEM:");
        pb.Add(text);
        return pb;
    }

    /// <summary>
    /// Adds an input section to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static InputSection Input(this PromptBuilder pb)
        => new(pb, pb);

    /// <summary>
    /// Adds a rules section to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static RulesSection Rules(this PromptBuilder pb)
        => new(pb, pb);

    /// <summary>
    /// Adds a context section to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static ContextSection Context(this PromptBuilder pb)
        => new(pb, pb);

    /// <summary>
    /// Adds a constraints section to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static ConstraintsSection Constraints(this PromptBuilder pb)
        => new(pb, pb);

    /// <summary>
    /// Adds an output section to the prompt.
    /// </summary>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static OutputSection Output(this PromptBuilder pb)
        => new(pb, pb);
}
