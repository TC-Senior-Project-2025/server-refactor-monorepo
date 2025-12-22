using System.Net.Http.Headers;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Common.Llm.Interfaces;
using Server.Common.Llm.Options;
using Server.Common.Llm.Services;

namespace Tests.Integration.Bases;

public abstract class LlmTestBase
{
    protected IServiceProvider Sp = default!;
    protected ILlmService Llm = default!;

    [OneTimeSetUp]
    public void LlmOneTimeSetup()
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

        // Register your ILlmService implementation
        services.AddHttpClient<ILlmService, OpenRouterLlmService>((sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<LlmOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(opts.Timeout);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", opts.ApiKey);
        });

        Sp = services.BuildServiceProvider(validateScopes: true);
        Llm = Sp.GetRequiredService<ILlmService>();
    }

    [OneTimeTearDown]
    public void LlmOneTimeTeardown()
    {
        if (Sp is IDisposable d) d.Dispose();
    }

    /// <summary>
    /// Helper: resolve SUT from container if you register it or create it manually here.
    /// </summary>
    protected T Get<T>() where T : notnull => Sp.GetRequiredService<T>();
}
