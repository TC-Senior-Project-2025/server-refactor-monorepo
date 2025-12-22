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

namespace Tests.Integration;

[TestFixture]
public class HistorySummarizerTests
{
    private ServiceProvider _sp = null!;
    private ILlmService _llmService = null!;
    private HistorySummarizer _sut = null!;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Env.Load();

        var config = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var apiKey = config["Llm:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            Assert.Ignore("Missing Llm:ApiKey. Set LLM__ApiKey or .env");

        // Sanity: does binding see it?
        var probe = new LlmOptions();
        config.GetSection("Llm").Bind(probe);
        if (string.IsNullOrWhiteSpace(probe.ApiKey))
            Assert.Fail("Config has Llm:ApiKey but binding to LlmOptions.ApiKey produced empty. Check LlmOptions property names/casing.");

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(config);
        services.AddLogging(b => b.AddConsole());

        services.AddOptions<LlmOptions>()
            .Bind(config.GetSection("Llm"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "Llm:ApiKey is required")
            .ValidateOnStart();

        services.AddHttpClient<ILlmService, OpenRouterLlmService>((sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<LlmOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(opts.Timeout);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", opts.ApiKey);
        });

        _sp = services.BuildServiceProvider();

        _llmService = _sp.GetRequiredService<ILlmService>();
        _sut = new HistorySummarizer(_sp.GetRequiredService<ILogger<HistorySummarizer>>(), _llmService);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _sp?.Dispose();
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
        const string history = """
                      Han stands as the smallest and most vulnerable of the seven Warring States. Sandwiched between the expansionist Qin to the west, powerful Wei to the north, and ambitious Chu to the south, Han’s survival depends on diplomacy, strategic positioning, and caution. Once a central Zhou territory, Han now acts more as a buffer state than a contender.
                      
                      Leadership: King Huanhui rules in name, but real power is diffused among ministers and regional elites. Han Fei, a brilliant Legalist thinker, influences policy circles but lacks formal authority. The royal court is cautious, with little appetite for reform that might anger nobles or Qin.
                      
                      Internal Affairs: The economy is modest. Han’s heartland in the Central Plains provides grain and access to trade, but manpower is low and recent territorial losses — such as Shangdang to Qin — have hurt morale. Central authority is weak, and local lords often act autonomously. Reforms are possible but risky.
                      
                      Military: Han maintains a small, largely defensive army. Fortified passes and river crossings offer natural advantages, but prolonged warfare would be ruinous. Without strong allies or hired troops, Han cannot survive direct assault by Qin.
                      
                      Diplomacy: Han’s best weapon is negotiation. It has ties to Wei and Zhao, and must avoid provoking Qin while quietly supporting any anti-Qin coalition. Bribery, flattery, and promises of access or supplies can buy Han time and security.
                      
                      Territory: Han controls a narrow corridor of the Central Plains, including Yingchuan and Sanchuan. Though small, this land is critical for movement between east and west — a fact that can be leveraged in diplomacy.
                      
                      Threats & Opportunities: Qin’s dominance threatens Han’s survival. However, by acting as a diplomatic broker and logistical keystone, Han can delay conquest and shape the battlefield indirectly. Survive the early game, and you may yet turn irrelevance into influence.
                      """;

        var summary = await _sut.SummarizeAsync(history);
        Assert.That(summary, Is.Not.Null.And.Not.Empty);
        await TestContext.Out.WriteLineAsync(summary);
    }
}