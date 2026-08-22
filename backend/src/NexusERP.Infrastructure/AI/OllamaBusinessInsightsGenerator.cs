using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NexusERP.Application.Common.Exceptions;

using Microsoft.Extensions.Options;

using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Infrastructure.AI;

public sealed class OllamaBusinessInsightsGenerator
    : IAiInsightsGenerator
{
    private readonly HttpClient _httpClient;
    private readonly OllamaSettings _settings;

    public OllamaBusinessInsightsGenerator(
        HttpClient httpClient,
        IOptions<OllamaSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        if (string.IsNullOrWhiteSpace(
                _settings.BaseUrl))
        {
            throw new InvalidOperationException(
                "Ollama base URL is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _settings.Model))
        {
            throw new InvalidOperationException(
                "Ollama model is not configured.");
        }

        _httpClient.BaseAddress =
            new Uri(
                _settings.BaseUrl.TrimEnd('/') +
                "/");
    }

    public async Task<string>
    GenerateBusinessInsightsAsync(
        IReadOnlyList<string> signals,
        CancellationToken cancellationToken = default)
{
    try
    {
        var request =
            new OllamaGenerateRequest(
                _settings.Model,
                BuildPrompt(signals),
                false);

        using var response =
            await _httpClient.PostAsJsonAsync(
                "api/generate",
                request,
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new AiProviderUnavailableException(
                $"AI provider returned status code {(int)response.StatusCode}.");
        }

        var result =
            await response.Content
                .ReadFromJsonAsync<OllamaGenerateResponse>(
                    cancellationToken:
                        cancellationToken);

        if (result is null ||
            string.IsNullOrWhiteSpace(
                result.Response))
        {
            throw new AiProviderUnavailableException(
                "AI provider returned an invalid response.");
        }

        return result.Response.Trim();
    }
    catch (HttpRequestException exception)
    {
        throw new AiProviderUnavailableException(
            "AI provider is unavailable.",
            exception);
    }
    catch (TaskCanceledException exception)
        when (!cancellationToken.IsCancellationRequested)
    {
        throw new AiProviderUnavailableException(
            "AI provider request timed out.",
            exception);
    }
}

    private static string BuildPrompt(
        IReadOnlyList<string> signals)
    {
        var builder =
            new StringBuilder();

        builder.AppendLine(
            "You are a writing assistant for NexusERP.");

        builder.AppendLine(
            "Write a very short business summary using only the supplied statements.");

        builder.AppendLine();

        builder.AppendLine(
            "STRICT RULES:");

        builder.AppendLine(
            "- Use only the supplied statements.");

        builder.AppendLine(
            "- Do not introduce numbers, amounts, percentages, names, or dates.");

        builder.AppendLine(
            "- Do not perform calculations.");

        builder.AppendLine(
            "- Do not infer causes, trends, forecasts, risks, delays, inconsistencies, or business outcomes.");

        builder.AppendLine(
            "- Do not invent additional recommendations.");

        builder.AppendLine(
            "- Do not contradict the supplied statements.");

        builder.AppendLine(
            "- Do not ask a follow-up question.");

        builder.AppendLine(
            "- Write no more than three short sentences.");

        builder.AppendLine();

        builder.AppendLine(
            "SUPPLIED STATEMENTS:");

        foreach (var signal in signals)
        {
            builder.AppendLine(
                $"- {signal}");
        }

        return builder.ToString();
    }

    private sealed record OllamaGenerateRequest(
        string Model,
        string Prompt,
        bool Stream);

    private sealed record OllamaGenerateResponse(
        string Response);
}