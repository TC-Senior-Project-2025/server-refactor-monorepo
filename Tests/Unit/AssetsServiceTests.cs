using Microsoft.Extensions.Logging;
using Moq;
using Server.Modules.Assets;

namespace Tests.Unit;

[TestFixture]
public class AssetsServiceTests
{
    private AssetsService _sut;
    private Mock<ILogger<AssetsService>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AssetsService>>();
        _sut = new AssetsService(_logger.Object);
    }

    [Test]
    public void LoadScenario_DefaultScenario_CurrentYear_Minus247()
    {
        var scenario = _sut.LoadScenario("default_scenario");
        Assert.That(scenario?.Game[0].CurrentYear, Is.EqualTo(-247));
    }

    [Test]
    public void LoadScenario_DefaultScenario_Has36Commanderies()
    {
        var scenario = _sut.LoadScenario("default_scenario");
        Assert.That(scenario?.Commandery.Count, Is.EqualTo(36));
    }
}