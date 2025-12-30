using Moq;
using Server.Common.Database;
using Server.Modules.Commanderies;
using Server.Modules.Countries;
using Server.Modules.SaveGames;
using Server.Modules.Scenarios;

namespace Tests.Unit;

[TestFixture]
public class ScenariosServiceTests
{
    private ScenariosService _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new ScenariosService();
    }

    [Test]
    public void LoadScenario_DefaultScenario_CurrentYear_Minus247()
    {
        var scenario = _sut.LoadScenario("default_scenario");
        Assert.That(scenario?.Game.CurrentYear, Is.EqualTo(-247));
    }
    
    [Test]
    public void LoadScenario_DefaultScenario_Has36Commanderies()
    {
        var scenario = _sut.LoadScenario("default_scenario");
        Assert.That(scenario?.Commanderies.Count, Is.EqualTo(36));
    }
}