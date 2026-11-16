using System.Diagnostics;
using DeepSeekIntegration.Models;
using DeepSeekIntegration.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeepSeekIntegration.Controllers;

public class HomeController(
    IDeepSeekService deepSeekService,
    ILogger<HomeController> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ChatViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ChatViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var sw = Stopwatch.StartNew();

        try
        {
            var (reply, totalTokens) = await deepSeekService.AskAsync(
                model.UserPrompt,
                model.SystemPrompt,
                cancellationToken);

            sw.Stop();

            model.AssistantReply = reply;
            model.TotalTokensUsed = totalTokens;
            model.ElapsedMilliseconds = sw.ElapsedMilliseconds;
        }
        catch (Exception ex)
        {
            sw.Stop();
            logger.LogError(ex, "Error calling DeepSeek-V4 endpoint.");
            model.ErrorMessage = $"Request failed: {ex.Message}";
        }

        return View(model);
    }
}