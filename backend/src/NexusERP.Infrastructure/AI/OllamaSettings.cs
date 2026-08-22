namespace NexusERP.Infrastructure.AI;

public sealed class OllamaSettings
{
    public const string SectionName = "Ollama";

    public string BaseUrl { get; init; } =
        "http://localhost:11434";

    public string Model { get; init; } =
        string.Empty;

    public int TimeoutSeconds { get; init; } =
        60;
}