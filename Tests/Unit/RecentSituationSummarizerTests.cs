using Moq;
using Server.Common.Llm.Interfaces;
using Server.Modules.Game.Core;
using Server.Modules.Game.Memory;

namespace Tests.Unit;

[TestFixture]
public class RecentSituationSummarizerTests
{
    private Mock<ILlmService> _llmMock = null!;
    private RecentSituationSummarizer _sut = null!;
    
    [SetUp]
    public void SetUp()
    {
        _llmMock = new Mock<ILlmService>();
        _sut = new RecentSituationSummarizer(_llmMock.Object);
    }
    
    [Test]
    public void BuildPromptTest()
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
        
        var prompt = RecentSituationSummarizer.BuildPrompt("Qin", 1, previousSummary, recentEvents, 5);
        TestContext.Out.WriteLine(prompt);
    }
}