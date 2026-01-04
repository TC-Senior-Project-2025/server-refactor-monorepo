using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Server.Modules;
using Server.Modules.Game;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Handlers;
using Server.Modules.Game.Core;
using ArmyUnit = Server.Modules.Game.Core.Unit;

namespace Tests.Unit;

[TestFixture]
public class SplitUnitsHandlerTests
{
    private SplitUnitsHandler _sut;
    private GameActionContext _mockGameActionContext;

    [SetUp]
    public void Setup()
    {
        _sut = new SplitUnitsHandler();
        _mockGameActionContext = new()
        {
            Payload = default!,
            Socket = default!,
            UserEntity = default!,
        };
    }

    [Test]
    public async Task HandleAsync_ValidSplit_SplitsUnitCorrectly()
    {
        // Arrange
        var sourceUnit = new ArmyUnit
        {
            Id = 1,
            CountryId = 1,
            CommanderId = 10,
            LocationId = 100,
            Name = "Infantry",
            Size = 1000,
            Morale = 80,
            Supply = 500,
            History = "Created",
            CanMove = true
        };

        var gameState = new GameState
        {
            Turn = 1,
            PlayerCountryId = 1,
            Countries = new Dictionary<int, Country>(),
            Commanderies = new Dictionary<int, Commandery>(),
            Units = new List<ArmyUnit> { sourceUnit },
            People = new List<Person>()
        };

        var dto = new SplitUnitDto { UnitId = 1 };

        // Act
        await _sut.HandleAsync(_mockGameActionContext, gameState, dto);

        // Assert
        Assert.That(gameState.Units.Count, Is.EqualTo(2));
        var original = gameState.Units.First(u => u.Id == 1);
        var split = gameState.Units.First(u => u.Id == 2);

        Assert.That(original.Size, Is.EqualTo(500));
        Assert.That(original.Supply, Is.EqualTo(250));

        Assert.That(split.Size, Is.EqualTo(500));
        Assert.That(split.Supply, Is.EqualTo(250));
        Assert.That(split.Name, Is.EqualTo("Infantry (Split)"));
        Assert.That(split.CommanderId, Is.Null);
        Assert.That(split.LocationId, Is.EqualTo(100));
        Assert.That(split.CountryId, Is.EqualTo(1));
    }

    [Test]
    public void HandleAsync_NonExistentUnit_ThrowsException()
    {
        var gameState = new GameState
        {
            Turn = 1,
            PlayerCountryId = 1,
            Countries = new Dictionary<int, Country>(),
            Commanderies = new Dictionary<int, Commandery>(),
            Units = new List<ArmyUnit>(),
            People = new List<Person>()
        };

        var dto = new SplitUnitDto { UnitId = 99 };

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _sut.HandleAsync(_mockGameActionContext, gameState, dto));
    }
}
