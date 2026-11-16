namespace DeepSeekIntegration.Services;

public interface IDeepSeekService
{
    Task<(string Reply, int TotalTokens)> AskAsync(
        string userPrompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default);
}