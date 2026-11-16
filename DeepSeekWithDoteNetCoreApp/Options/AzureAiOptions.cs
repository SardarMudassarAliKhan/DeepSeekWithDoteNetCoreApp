using System.ComponentModel.DataAnnotations;

namespace DeepSeekIntegration.Options;

public class AzureAiOptions
{
    public const string SectionName = "AzureAI";

    [Required, Url]
    public required string Endpoint { get; init; }

    [Required]
    public required string DeploymentName { get; init; }

    public string? ApiKey { get; init; }
}