using System.Text.Json;
using Server.Common.Llm.Errors;
using Server.Common.Llm.Interfaces;
using Server.Modules.Game.Builders.Prompt;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Events;

/// <summary>
/// EventGenerator is responsible for the generation of events using an LLM service.
/// </summary>
public class EventGenerator(ILogger<EventGenerator> logger, ILlmService llmService)
{
    /// <summary>
    /// Generates an event using the LLM service by sending a predefined prompt and retrieving the response.
    /// If the LLM service fails to provide a valid response after the specified number of retries, a fallback value is returned.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    /// <param name="maxRetries">The maximum number of retry attempts allowed if a valid response is not received. Defaults to 3.</param>
    /// <returns>A task representing the asynchronous operation, containing the generated event as a string. Returns a fallback event if the LLM service fails to respond.</returns>
    public async Task<GameEvent> GenerateEventAsync(GameState gameState, int maxRetries = 3)
    {
        var numRetries = 0;
        var prompt = BuildEventPrompt(gameState);
        while (numRetries++ < maxRetries)
        {
            try
            {
                var response = await llmService.AskText(prompt);
                if (response.Choices.Count == 0)
                {
                    logger.LogWarning("No choices received from LLM.");
                    continue;
                }
                var gameEventJson = response.Choices[0].Message.Content;
                
                var gameEvent = JsonSerializer.Deserialize<GameEvent>(gameEventJson);
                if (gameEvent == null)
                {
                    logger.LogWarning("Failed to deserialize event JSON: {}", gameEventJson);
                    continue;
                };
                
                return gameEvent;
            }
            catch (LlmMaxRetriesException e)
            {
                logger.LogWarning("Max retries: {}", e);
            }
            catch (Exception e)
            {
                logger.LogWarning("Other exception: {}", e);
            }
        }
        return new GameEvent
        {
            Title = "Fallback Event",
            Description =
                "If you see this, it means that the system has failed to generate an event! We apologize for your inconvenience.",
            ResourceChanges = NationalResources.Zero(),
        };
    }

    public async Task<GameEvent> GenerateEventExampleAsync()
    {
        await Task.Delay(1000);
        return new GameEvent
        {
            Title = "Test Event",
            Description = "Description goes here.",
            Options = [
                new GameEventOption
                {
                    Title = "Option 1",
                    Description = "This is an example option."
                }
            ],
            ResourceChanges = new NationalResources
            {
                Efficiency = -5,
                Treasury = 999,
                Manpower = 100,
                Stability = -5,
                Prestige = -5
            }
        };
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxRetries"></param>
    /// <returns></returns>
    public async Task<GameEventOptionEffects> GenerateEventOptionEffectsAsync(GameEventOption option, int maxRetries = 3)
    {
        var numRetries = 0;
        var prompt = BuildEventOptionEffectsPrompt(option);
        while (numRetries++ < maxRetries)
        {
            try
            {
                var response = await llmService.AskText(prompt);
                if (response.Choices.Count == 0)
                {
                    logger.LogWarning("No choices received from LLM.");
                    continue;
                }
                var effectsJson = response.Choices[0].Message.Content;
                
                logger.LogInformation("Received effects JSON: {}", effectsJson);
                
                var effects = JsonSerializer.Deserialize<GameEventOptionEffects>(effectsJson);
                if (effects == null)
                {
                    logger.LogWarning("Failed to deserialize effects JSON: {}", effectsJson);
                    continue;
                };
                
                return effects;
            }
            catch (LlmMaxRetriesException e)
            {
                logger.LogWarning("Max retries: {}", e);
            }
            catch (Exception e)
            {
                logger.LogWarning("Other exception: {}", e);
            }
        }
        return new GameEventOptionEffects
        {
            Title = "Fallback Effect",
            Description = "If you see this, it means that the system has failed to generate option effects! We apologize for your inconvenience.",
            FlagChangeset = new FlagChangeset(),
            ResourceChanges = NationalResources.Zero()
        };
    }

    public async Task<GameEventOptionEffects> GenerateEventOptionEffectsExampleAsync()
    {
        await Task.Delay(1000);
        return new GameEventOptionEffects
        {
            Title = "Test Effect",
            Description = "This is a test.",
            FlagChangeset = new FlagChangeset(),
            ResourceChanges = new NationalResources
            {
                Efficiency = 5,
                Treasury = -500,
                Manpower = -100,
                Stability = -30,
                Prestige = -20
            },
        };
    }

    /// <summary>
    /// Builds the prompt string.
    /// </summary>
    /// <returns></returns>
    public static string BuildEventPrompt(GameState gameState)
    {
        var eventExample = new GameEvent
        {
            Title = "Event Title (TitleCase)",
            Description = "Event description (should be less than 10 sentences)",
            Options = [
                new GameEventOption
                {
                    Title = "Option 1 (Normal case)",
                    Description = "Option 1 description"
                }
            ], ResourceChanges = NationalResources.Zero()
        };
        
        return PromptBuilder
            .Create()
            .AsSystem("You are an event generator for a turn-based nation management game set during the Chinese Warring States period. You generate plausible, historically grounded events.")
            .Context()
                .Bullet($"Nation Code: {gameState.PlayerCountryCode}")
                .Bullet($"Turn: {gameState.Turn}")
                .Bullet($"Recent Situation Summary (may be empty): {gameState.GetPlayerCountry().RecentSituationSummary}")
            .End()
            .Rules()
                .UseSchemaExample(eventExample)
                .Bullet("Output must be a one-line JSON object.")
                .Bullet("Generate concise descriptions for the event.")
                .Bullet("Feel free to leave options empty (as an empty array) if it makes sense.")
                .Bullet("Resource changes should correlate to the event contents (title, description).")
                .JsonOnly()
                .NoMarkdown()
            .End()
            .Constraints()
                .MaxOptions(3)
            .End()
            .Output()
                .Head("Output must be a one-line JSON object.")
            .End()
            .Build();
    }

    /// <summary>
    /// Builds a prompt designed to generate effects for a game event option using a predefined structure and rules.
    /// The prompt serves as input for an AI system to create concise and relevant option effects.
    /// </summary>
    /// <param name="option">The game event option for which effects need to be generated. Includes title and description.</param>
    /// <returns>A formatted string representing the prompt to be used for AI-based generation of game event option effects.</returns>
    public static string BuildEventOptionEffectsPrompt(GameEventOption option)
    {
        var effectsExample = new GameEventOptionEffects
        {
            Title = "Effect Title (TitleCase)",
            Description = "Effect description (should be less than 10 sentences)",
            FlagChangeset =
                new FlagChangeset
                {
                    Add =
                    [
                        "AtWar.Qin"
                    ],
                    Remove =
                    [
                        "PromiseOfPeace"
                    ]
                },
            ResourceChanges = NationalResources.Zero(),
        };
        
        return PromptBuilder
            .Create()
            .AsSystem("You are an event option effects generator given option for a text-based, nation management game set during the Chinese Warring States period.")
            .Input()
                .Bullet($"Option title: {option.Title}")
                .Bullet($"Option description: {option.Description}")
            .End()
            .Rules()
                .UseSchemaExample(effectsExample)
                .Bullet("Generate concise effect descriptions.")
                .Bullet("Feel free to leave flags empty (as an empty array) if it makes sense.")
                .JsonOnly()
                .NoMarkdown()
            .End()
            .Constraints()
                .MaxOptions(3)
            .End()
            .Output()
                .Head("Output must be a one-line JSON object.")
            .End()
            .Build();
    }
}