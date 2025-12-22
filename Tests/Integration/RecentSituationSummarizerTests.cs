using Server.Modules.Game.Core;
using Server.Modules.Game.Memory;
using Tests.Integration.Bases;

namespace Tests.Integration;

[TestFixture]
public class RecentSituationSummarizerTests : LlmTestBase
{
    private RecentSituationSummarizer _sut = default!;

    [SetUp]
    public void Setup()
    {
        _sut = new RecentSituationSummarizer(Llm);
    }
    
    [Test]
    public async Task SummarizeAsync_ReturnsSummary()
    {
        const string previousSummary =
            "Qin continues to consolidate power under the regency of Lü Buwei, strengthening central " +
            "authority and military readiness. Neighboring states remain wary, but no coordinated " +
            "opposition has yet formed.";
        
        List<RecentEvent> recentEvents =
        [
            new(
                Title: "Border Commanderies Reorganized",
                Description:
                "The Qin court approved a restructuring of newly conquered border territories into " +
                "formal commanderies, placing them under centrally appointed officials.",
                ChosenOptionTitle: "Strengthen Central Oversight",
                ChosenOptionDescription:
                "Appoint trusted Legalist administrators to secure loyalty and improve tax collection."
            ),

            new(
                Title: "Envoys from Wei Seek Reassurance",
                Description:
                "Wei dispatched envoys to Xianyang, expressing concern over Qin troop movements near " +
                "the Yellow River and requesting assurances of peaceful intent.",
                ChosenOptionTitle: "Offer Vague Assurances",
                ChosenOptionDescription:
                "Calm Wei’s fears without committing to any binding agreement."
            )
        ];

        const int turn = 1;
        var summary = await _sut.SummarizeAsync("Qin", turn, previousSummary, recentEvents);
        await TestContext.Out.WriteLineAsync(summary);
    }
}