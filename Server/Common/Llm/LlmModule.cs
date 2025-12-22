using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Server.Common.Llm.Interfaces;
using Server.Common.Llm.Options;
using Server.Common.Llm.Services;

namespace Server.Common.Llm;

/// <summary>
/// Module for registering LLM services.
/// </summary>
public static class LlmModule
{
    /// <summary>
    /// Registers LLM services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<LlmOptions>()
            .Bind(configuration.GetSection("Llm"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey),
                "Llm:ApiKey is required")
            .ValidateOnStart();
        
        services.AddHttpClient<ILlmService, OpenRouterLlmService>((sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<LlmOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(opts.Timeout);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opts.ApiKey);
        });
    }
}
