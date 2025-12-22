namespace Server.Modules.Game.Builders.Prompt;

/// <summary>
/// Represents a base class for defining structured sections within an event prompt.
/// Each section facilitates the creation of a specific part of an event prompt, providing a
/// consistent structure and interface for extending the EventPromptBuilder.
/// </summary>
/// <typeparam name="TParent">
/// The type of the parent object, allowing hierarchical navigation back to the parent builder or section.
/// </typeparam>
public abstract class PromptSection<TParent>
{
    /// <summary>
    /// The parent builder.
    /// </summary>
    protected readonly PromptBuilder Builder;

    /// <summary>
    /// The parent section.
    /// </summary>
    protected readonly TParent Parent;

    /// <summary>
    /// Creates a new section.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="parent"></param>
    protected PromptSection(PromptBuilder builder, TParent parent)
    {
        Builder = builder;
        Parent = parent;
    }

    /// <summary>
    /// Ends the section and returns the parent builder.
    /// </summary>
    /// <returns></returns>
    public TParent End() => Parent;
}
