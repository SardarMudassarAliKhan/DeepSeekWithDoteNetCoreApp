using System.ClientModel;
using System.ClientModel.Primitives;
using Azure.Identity;
using DeepSeekIntegration.Options;
using DeepSeekIntegration.Services;
using Microsoft.Extensions.Options;
using OpenAI;

namespace DeepSeekIntegration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services
            .AddOptions<AzureAiOptions>()
            .BindConfiguration(AzureAiOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

#pragma warning disable OPENAI001
        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<AzureAiOptions>>().Value;
            var clientOptions = new OpenAIClientOptions
            {
                Endpoint = new Uri(settings.Endpoint)
            };

            if (!string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                return new OpenAIClient(new ApiKeyCredential(settings.ApiKey), clientOptions);
            }

            var tokenPolicy = new BearerTokenPolicy(
                new DefaultAzureCredential(),
                "https://cognitiveservices.azure.com/.default");

            return new OpenAIClient(authenticationPolicy: tokenPolicy, options: clientOptions);
        });
#pragma warning restore OPENAI001

        builder.Services.AddScoped<IDeepSeekService, DeepSeekService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}