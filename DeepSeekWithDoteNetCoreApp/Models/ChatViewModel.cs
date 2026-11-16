using System.ComponentModel.DataAnnotations;

namespace DeepSeekIntegration.Models;

public class ChatViewModel
{
    [Display(Name = "System Instructions (Optional)")]
    public string SystemPrompt { get; set; } = "You are a helpful AI assistant.";

    [Required(ErrorMessage = "Please enter a message before sending.")]
    [Display(Name = "Your Prompt")]
    public string UserPrompt { get; set; } = string.Empty;

    public string? AssistantReply { get; set; }
    public int? TotalTokensUsed { get; set; }
    public long? ElapsedMilliseconds { get; set; }
    public string? ErrorMessage { get; set; }
}