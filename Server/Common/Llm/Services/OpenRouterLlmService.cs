using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Server.Common.Llm.Dtos;
using Server.Common.Llm.Enums;
using Server.Common.Llm.Errors;
using Server.Common.Llm.Interfaces;
using Server.Common.Llm.Options;

namespace Server.Common.Llm.Services;

/// <summary>
/// OpenRouter LLM service.
/// </summary>
public class OpenRouterLlmService(
    HttpClient httpClient, 
    ILogger<OpenRouterLlmService> logger
    ) : ILlmService
{
    /// <inheritdoc/>
    public async Task<LlmApiResponse> AskText(string prompt, LlmModel model = LlmModel.OpenRouterGpt52Chat, int maxRetries = 3)
    {
        const string url = "https://openrouter.ai/api/v1/chat/completions";
        var attempt = 0;
        Exception? lastException = null;
        var modelString = model.AsString();
        logger.LogInformation("Asking {ModelString} with prompt: {Prompt}", modelString, prompt);
        
        while (attempt < maxRetries)
        {
            logger.LogInformation("Attempt {Attempt} of {MaxRetries}", attempt + 1, maxRetries);
            try
            {
                var payload = new
                {
                    model = modelString,
                    messages = new object[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new { type = "text", text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContentText = await response.Content.ReadAsStringAsync();
                    var responseContent = JsonSerializer.Deserialize<LlmApiResponse>(responseContentText) ??
                                          throw new Exception("Failed to deserialize response content");
                    return responseContent;
                }
                else if (IsRetryableStatusCode(response.StatusCode))
                {
                    logger.LogWarning("Encountered a retryable status code: {StatusCode}", response.StatusCode);
                    
                    lastException = new Exception($"HttpError: {response.StatusCode}\nResponse: {await response.Content.ReadAsStringAsync()}");
                    attempt++;

                    if (attempt < maxRetries)
                    {
                        await Task.Delay(CalculateBackoffDelay(attempt));
                    }
                }
                else
                {
                    throw new Exception($"HttpError: {response.StatusCode}\nResponse: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (HttpRequestException ex)
            {
                logger.LogWarning("Encountered a HttpRequestException: {Message}", ex.Message);
                lastException = ex;
                attempt++;

                if (attempt < maxRetries)
                {
                    await Task.Delay(CalculateBackoffDelay(attempt));
                }
            }
            catch (TaskCanceledException ex)
            {
                logger.LogWarning("Task cancelled: {Message}", ex.Message);
                lastException = ex;
                attempt++;

                if (attempt < maxRetries)
                {
                    await Task.Delay(CalculateBackoffDelay(attempt));
                }
            }
        }

        throw new LlmMaxRetriesException(maxRetries, lastException);
    }

    /// <summary>
    /// Determines if an HTTP status code is retryable.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns>True if the status code is retryable, otherwise false.</returns>
    private static bool IsRetryableStatusCode(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.TooManyRequests
            or HttpStatusCode.InternalServerError
            or HttpStatusCode.BadGateway
            or HttpStatusCode.ServiceUnavailable
            or HttpStatusCode.GatewayTimeout;
    }

    /// <summary>
    /// Calculates the exponential backoff delay for retry attempts.
    /// </summary>
    /// <param name="attempt">The current attempt number (1-based).</param>
    /// <returns>The delay duration in milliseconds.</returns>
    private static TimeSpan CalculateBackoffDelay(int attempt)
    {
        return TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 1000);
    }
}