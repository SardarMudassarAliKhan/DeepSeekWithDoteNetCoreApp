using DeepSeekIntegration.Options;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace DeepSeekIntegration.Services;

public class DeepSeekService(
    OpenAIClient openAiClient,
    IOptions<AzureAiOptions> options) : IDeepSeekService
{
    private readonly ChatClient _chatClient = openAiClient.GetChatClient(options.Value.DeploymentName);

    public async Task<(string Reply, int TotalTokens)> AskAsync(
        string userPrompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default)
    {
        List<ChatMessage> messages = [];

        if (!string.IsNullOrWhiteSpace(systemPrompt))
        {
            messages.Add(new SystemChatMessage(systemPrompt));
        }

        messages.Add(new UserChatMessage(userPrompt));

        var completionOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 2048,
            Temperature = 0.7f
        };

        ChatCompletion response = await _chatClient.CompleteChatAsync(
            messages,
            completionOptions,
            cancellationToken);

        string text = response.Content.FirstOrDefault()?.Text ?? string.Empty;
        int totalTokens = response.Usage?.TotalTokenCount ?? 0;

        return (text, totalTokens);
    }
}