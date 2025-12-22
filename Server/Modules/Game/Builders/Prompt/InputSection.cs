namespace Server.Modules.Game.Builders.Prompt;

public sealed class InputSection : PromptSection<PromptBuilder>
{
    public InputSection(PromptBuilder b, PromptBuilder p) : base(b, p)
    {
        Builder.Add("INPUT:");
    }

    public InputSection Head(string input)
    {
        Builder.Add($"{input}");
        return this;
    }
    
    public InputSection Bullet(string input)
    {
        Builder.Add($"- {input}");
        return this;
    }
}
