using System.Net.Http.Headers;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Common.Llm.Interfaces;
using Server.Common.Llm.Options;
using Server.Common.Llm.Services;
using Server.Modules.Game.Memory;
using Tests.Integration.Bases;

namespace Tests.Integration;

[TestFixture]
public class HistorySummarizerTests : LlmTestBase
{
    private HistorySummarizer _sut;
    private ILogger<HistorySummarizer> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = Sp.GetRequiredService<ILogger<HistorySummarizer>>();
        _sut = new HistorySummarizer(_logger, Llm);
    }
    
    [Test]
    public void TestPrompt()
    {
        var prompt = _sut.BuildPrompt("Test history");
        Assert.That(prompt, Is.Not.Null.And.Not.Empty);
        TestContext.Out.WriteLine(prompt);
    }

    [Test]
    public async Task SummarizeAsync_ReturnsSummary()
    {
        const string history = "Leadership: King Kaolie (Xiong Wan/Yuan, r. 262–238 BC) sits on the Chu throne . His chief\nminister is Lord Chunshen (Huang Xie), an astute veteran who has been Chu’s Prime Minister and\none of the famed “Four Lords” of the period . Queen Dowager and court aristocrats also influence\npolicies.\nInternal: Chu is the largest state by land and population. Its economy is based on rice farming in the\nsouth and loess agriculture up north. Kaolie’s government is stable, absorbing the recent annexation\nof Lu (249 BC) . Chunshen has invited many scholars and retainers to court, boosting Chu’s\nculture. However, internal factionalism (nobles vs. central court) and a large bureaucracy mean\nreforms are slow.\nMilitary: Chu’s army is vast, drawing on loyal peasant conscripts and nobles’ cavalry. It boasts elite\ntroops (e.g. Lord Chunshen himself is a capable general). Recently Chu successfully absorbed Lu ,\nextending its border east. Though once repeatedly checked by Qin, Chu still fields the largest\ninfantry. Generals like Zhuang Qiao and Li You stand ready at frontier garrisons.\nDiplomacy: Chu often leads southern coalitions. It maintains ties with Wei and Zhao against Qin and\nhas friendly trade with Qi. However, it remains suspicious of northern neighbors. There are no formal\nalliances now (some rivals fear Chu’s expansion) but Chu’s reputation causes others to court or avoid\nit.\nTerritory: Chu rules all lands between the Huai and Yangtze rivers and south to the Han River basin.\nIts effective capital is in Shouchun (moved from Yinglong/Shou in past decades to stay away from\nQin). Chu holds rich rice fields and strategic southern passes (e.g. at Shouxian). It also commands\nfrontier corridors into the Yangtze valleys.\nThreats/Opportunities: Qin’s looming power in the west is Chu’s chief concern. Chu’s leaders must\nguard against Qin’s next move. Internally, however, Chu’s size is its strength – it can field armies large\nenough to strike at weaker neighbors (an opportunity Chu has used to expand). With careful\ndiplomacy, Chu could be pivot in any anti‑Qin alliance while retaining its dominion over southern\nChina.";

        var summary = await _sut.SummarizeAsync(history);
        Assert.That(summary, Is.Not.Null.And.Not.Empty);
        await TestContext.Out.WriteLineAsync(summary);
    }
}