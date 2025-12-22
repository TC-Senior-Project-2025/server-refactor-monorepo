namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Represents the context section in the event prompt-building process. This section
/// allows for defining contextual elements such as turn numbers, custom rules, and
/// narrative summaries. It provides methods for adding structured details relevant to
/// the context of the prompt being generated.
/// </summary>
public sealed class ContextSection : PromptSection<PromptBuilder>
{
    /// <summary>
    /// Creates a context section.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="p"></param>
    public ContextSection(PromptBuilder b, PromptBuilder p) : base(b, p)
    {
        Builder.Add("CONTEXT:");
    }

    /// <summary>
    /// Adds a turn number to the context section.
    /// </summary>
    /// <param name="turn"></param>
    /// <returns></returns>
    public ContextSection Turn(int turn)
    {
        Builder.Add($"- Turn: {turn}");
        return this;
    }
    //
    // public ContextSection Resources(NationalResources r)
    // {
    //     Builder.Add(
    //         $"- Resources: treasury={r.Treasury}, manpower={r.Manpower}, stability={r.Stability}, prestige={r.Prestige}, efficiency={r.Efficiency}"
    //     );
    //     return this;
    // }
    
    public ContextSection Head(string context)
    {
        Builder.Add($"{context}");
        return this;
    }
    
    public ContextSection Bullet(string context)
    {
        Builder.Add($"- {context}");
        return this;
    }

    // public ContextSection FocusNation(NationRelation n)
    // {
    //     Builder.Add(
    //         $"FocusNation: id={n.NationId}, opinion={n.Opinion}, atWar={n.AtWar}"
    //     );
    //     return this;
    // }

    /// <summary>
    /// Adds a narrative summary to the context section.
    /// </summary>
    /// <param name="summary"></param>
    /// <returns></returns>
    public ContextSection NarrativeSummary(string summary)
    {
        if (!string.IsNullOrWhiteSpace(summary))
            Builder.Add($"- Summary: {summary}");
        return this;
    }
}
