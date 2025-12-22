using System.Text.Json;

namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Represents a section within an event prompt specifically designed for defining rules.
/// This class allows for the addition of structured and customizable rules that dictate
/// the behavior or format of the event output. Supports predefined rules such as JSON-only
/// output or schema adherence, as well as the inclusion of custom rules.
/// </summary>
public sealed class RulesSection : PromptSection<PromptBuilder>
{
    /// <summary>
    /// Creates a rules section.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="p"></param>
    public RulesSection(PromptBuilder b, PromptBuilder p) : base(b, p)
    {
        Builder.Add("RULES:");
    }

    /// <summary>
    /// Adds a rule that specifies that the output should be a single JSON object.
    /// </summary>
    /// <returns></returns>
    public RulesSection JsonOnly()
    {
        Builder.Add("- Output JSON only.");
        return this;
    }

    /// <summary>
    /// Adds a rule that specifies that the output should follow a specific schema.
    /// </summary>
    /// <param name="schemaExample"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public RulesSection UseSchemaExample<T>(T schemaExample)
    {
        var json = JsonSerializer.Serialize(schemaExample);
        Builder.Add($"- Follow schema: {json}.");
        return this;
    }

    /// <summary>
    /// Adds a rule that specifies that the output should not contain any markdown or comments.
    /// </summary>
    /// <returns></returns>
    public RulesSection NoMarkdown()
    {
        Builder.Add("- No markdown or commentary.");
        return this;
    }

    /// <summary>
    /// Adds a custom rule to the section.
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    public RulesSection Bullet(string rule)
    {
        Builder.Add($" - {rule}");
        return this;
    }
}
