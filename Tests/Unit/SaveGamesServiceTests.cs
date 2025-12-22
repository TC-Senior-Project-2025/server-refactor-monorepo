using System.Text.Json;
using Server.Modules.Game.Core;

namespace Tests.Unit;

[TestFixture]
public class SaveGamesServiceTests
{
    [Test]
    public void SerializeGameState()
    {
        var gameState = new GameState
        {
            SaveId = null,
            Turn = 1,
            PlayerCountryCode = "QIN",
            Countries = new Dictionary<string, Country>()
            {
                { "QIN", new Country
                    {
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
        
        var serializedGameState = JsonSerializer.Serialize(gameState);
        Assert.That(serializedGameState, Is.Not.Null.And.Not.Empty);
        
        TestContext.Out.WriteLine(serializedGameState);
    }
}