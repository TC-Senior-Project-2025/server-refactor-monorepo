using Server.Common.Llm.Interfaces;
using Server.Modules.Game.Builders.Prompt;

namespace Server.Modules.Game.Memory;

public class HistorySummarizer(ILogger<HistorySummarizer> logger, ILlmService llmService)
{
    public async Task<string> SummarizeAsync(string history)
    {
        var prompt = BuildPrompt(history);
        try
        {
            var result = await llmService.AskText(prompt);
            if (result.Choices.Count == 0)
            {
                throw new Exception("No choices received from LLM.");
            };
            return result.Choices[0].Message.Content;
        }
        catch (Exception e)
        {
            logger.LogError("Error summarizing history: {}", e.Message);
            return "";
        }
    }

    public string BuildPrompt(string history)
    {
        var prompt = PromptBuilder
            .Create()
            .AsSystem("""
                      You are a narrative summarization engine for a text-based historical grand strategy game set during the Chinese Warring States period.
                      Your task is to condense long-form state histories into concise, factual summaries
                      suitable for player-facing UI and AI context grounding.
                      """)
            .Input()
                .Bullet($"""
                        FULL HISTORY TEXT:
                        {history}
                        """)
            .End()
            .Constraints()
                .Bullet("Output length: 60–90 words.")
                .Bullet("Single paragraph.")
                .Bullet("No bullet points.")
                .Bullet("No markdown.")
                .Bullet("No quotation marks.")
            .End()
            .Output()
                .Head("Output a single concise summary describing:")
                    .Bullet("Political situation")
                    .Bullet("Military posture")
                    .Bullet("Diplomatic position")
                    .Bullet("Strategic threats or opportunities")
            .End()
            .Build();
        return prompt;
    }
}