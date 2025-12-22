using Microsoft.Extensions.Logging;
using Moq;
using Server.Common.Llm.Interfaces;
using Server.Modules.Game.Core;
using Server.Modules.Game.Events;

namespace Tests.Unit;

[TestFixture]
public class EventGeneratorTests
{
    private Mock<ILogger<EventGenerator>> _loggerMock = null!;
    private Mock<ILlmService> _llmMock = null!;
    private EventGenerator _sut = null!;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<EventGenerator>>();
        _llmMock = new Mock<ILlmService>();
        _sut = new EventGenerator(_loggerMock.Object, _llmMock.Object);
    }
    
    [Test]
    public void BuildEventPromptTest()
    {
        var gameState = new GameState
        {
            SaveId = null,
            Turn = 1,
            PlayerCountryCode = "QIN",
            Countries = new Dictionary<string, Country>()
            {
                { "QIN", new Country() {
                        Code = "QIN",
                        Name = "Qin",
                        Efficiency = 0,
                        Treasury = 0,
                        Stability = 0,
                        Manpower = 0,
                        Prestige = 0
                    } 
                }
            },
            CurrentGameEvent = null
        };
        
        var prompt = EventGenerator.BuildEventPrompt(gameState);
        Assert.That(prompt, Is.Not.Null.And.Not.Empty);
        TestContext.Out.WriteLine(prompt);
    }
    
    [Test]
    public void BuildEventOptionEffectsPromptTest()
    {
        var option = new GameEventOption
        {
            Title = "Refuse their demands",
            Description = "An obvious act of aggression!"
        };

        var prompt = EventGenerator.BuildEventOptionEffectsPrompt(option);
        Assert.That(prompt, Is.Not.Null.And.Not.Empty);
        TestContext.Out.WriteLine(prompt);
    }
}