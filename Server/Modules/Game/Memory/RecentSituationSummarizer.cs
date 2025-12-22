using System.Text.Json;
using Server.Common.Llm.Interfaces;
using Server.Modules.Game.Builders.Prompt;

namespace Server.Modules.Game.Memory;

/// <summary>
/// Produces a short "Recent Situation Summary" (2–4 sentences) from recent events + current state,
/// intended for grounding future event generation prompts.
/// </summary>
public sealed class RecentSituationSummarizer(ILlmService llmService)
{
    // Keep it tiny + deterministic-ish.
    private const int DefaultMaxSentences = 4;

    /// <summary>
    /// Generates a short situation summary (plain text) for use in prompts.
    /// </summary>
    public async Task<string> SummarizeAsync(
        string nationName,
        int turn,
        string? previousSummary,
        IReadOnlyList<RecentEventForSummary> recentEvents,
        int maxSentences = DefaultMaxSentences,
        int maxRetries = 2)
    {
        // You can tweak this schema to match your ILlmService contract.
        // The idea: keep output strict and easy to validate.
        var prompt = BuildPrompt(nationName, turn, previousSummary, recentEvents, maxSentences);

        for (var attempt = 1; attempt <= Math.Max(1, maxRetries + 1); attempt++)
        {
            var raw = await llmService.AskText(prompt);
            if (raw.Choices.Count == 0) continue;
            
            return raw.Choices[0].Message.Content;
            
            // Retry with a tiny “self-correct” nudge.
            // prompt = BuildRepairPrompt(prompt, raw);
        }

        // Fallback: deterministic, minimal. Better than failing the turn.
        return BuildFallback(nationName, turn, previousSummary, recentEvents, maxSentences);
    }

    public static string BuildPrompt(
        string nationName,
        int turn,
        string? previousSummary,
        IReadOnlyList<RecentEventForSummary> recentEvents,
        int maxSentences)
    {
        // Keep input small: last few events only.
        // You can pre-trim to N events at call site if you want.
        var eventsJson = JsonSerializer.Serialize(recentEvents, new JsonSerializerOptions
        {
            WriteIndented = false
        });
        
        return PromptBuilder
            .Create()
            .AsSystem("""
                You are a narrative summarization engine for a historical grand strategy game.
                Produce a concise, factual "Recent Situation Summary" to ground the next turn's event generation.
                """)
            .Context()
                .Bullet($"Nation: {nationName}")
                .Bullet($"Turn: {turn}")
                .Bullet($"Previous Summary (may be empty): {previousSummary ?? ""}")
                .Bullet($"Recent Events (JSON): {eventsJson}")
            .End()
            .Rules()
                .Bullet("Do NOT invent new events, people, outcomes, or numbers.")
                .Bullet("Use neutral historical tone. No meta commentary.")
                .Bullet("The summary must be plain text (no bullet points).")
                .Bullet($"Keep it to {Math.Clamp(maxSentences, 1, 6)} sentences maximum.")
                .NoMarkdown()
                // .UseSchemaExample()
            .End()
            .Output()
                .Head("Output one-line summary text.")
            .End()
            .Build();
    }

    private static string BuildRepairPrompt(string originalPrompt, string badOutput)
    {
        // Instruct the model to fix itself while preserving the same constraints.
        return PromptBuilder
            .Create()
            .AsSystem("You fix invalid LLM outputs to match the required JSON schema exactly.")
            .Input()
                .Bullet("ORIGINAL PROMPT:")
                .Head($"{originalPrompt}")
                .Bullet("BAD OUTPUT:")
                .Head($"{badOutput}")
            .End()
            .Rules()
                .Bullet("Return only corrected JSON. No commentary.")
                .Bullet("Must match schema exactly: {\"Summary\":\"...\"}")
                .JsonOnly()
                .NoMarkdown()
            .End()
            .Output()
                .Head("Return corrected one-line JSON only.")
            .End()
            .Build();
    }

    private static string BuildFallback(
        string nationName,
        int turn,
        string? previousSummary,
        IReadOnlyList<RecentEventForSummary> recentEvents,
        int maxSentences)
    {
        // Simple deterministic summary: last event + carry forward.
        var last = recentEvents.LastOrDefault();
        var lastLine = last is null
            ? "Recent developments are unclear."
            : $"Most recently, {last.Title.ToLowerInvariant()}.";

        var baseLine = $"On turn {turn}, {nationName} faces ongoing pressures that shape near-term decisions.";

        // Keep it short.
        var summary = string.IsNullOrWhiteSpace(previousSummary)
            ? $"{baseLine} {lastLine}"
            : $"{previousSummary.Trim()} {lastLine}";

        // Hard trim by sentences (cheap + safe).
        return TrimToSentences(summary, Math.Clamp(maxSentences, 1, 6));
    }

    private static string TrimToSentences(string text, int maxSentences)
    {
        // Very naive sentence splitter; good enough for fallback.
        var parts = text.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length <= maxSentences) return text.Trim();

        var kept = parts.Take(maxSentences).Select(p => p.Trim());
        return string.Join(". ", kept) + ".";
    }
}

/// <summary>
/// Minimal event info for summarization. Keep this small to save tokens.
/// </summary>
public sealed record RecentEventForSummary(
    string Title,
    string Description,
    string? ChosenOptionTitle,
    string? ChosenOptionDescription);