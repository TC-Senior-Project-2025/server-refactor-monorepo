namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Represents the output section of an event prompt within the EventPromptBuilder framework.
/// Provides methods for specifying the expected format or structure of the output, such as JSON representations.
/// </summary>
public sealed class OutputSection : PromptSection<PromptBuilder>
{
    /// <summary>
    /// Creates an output section.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="p"></param>
    public OutputSection(PromptBuilder b, PromptBuilder p) : base(b, p)
    {
        Builder.Add("OUTPUT:");
    }

    /// <summary>
    /// Specifies that the output should be a single JSON object representing the event.
    /// </summary>
    /// <returns></returns>
    public OutputSection EventJson()
    {
        Builder.Add("- Single JSON object representing the event.");
        return this;
    }

    /// <summary>
    /// Custom output section.
    /// </summary>
    /// <returns></returns>
    public OutputSection Head(string custom)
    {
        Builder.Add($"{custom}");
        return this;
    }
    
    /// <summary>
    /// Custom output section with a bullet.
    /// </summary>
    /// <returns></returns>
    public OutputSection Bullet(string custom)
    {
        Builder.Add($"- {custom}");
        return this;
    }
}
