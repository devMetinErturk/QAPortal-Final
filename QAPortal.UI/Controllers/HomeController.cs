using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QAPortal.UI.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata.Ecma335;

namespace QAPortal.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Details()
    {
        return View();
    }

    public async Task<IActionResult> Questions()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("QAPortalAPI");
            var response = await client.GetAsync("questions");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };

                var questions = JsonSerializer.Deserialize<List<QuestionViewModel>>(content, options);
                return View(questions ?? new List<QuestionViewModel>());
            }

            return View(new List<QuestionViewModel>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching questions");
            return View(new List<QuestionViewModel>());
        }
    }

    public async Task<IActionResult> Tags()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("QAPortalAPI");
            var response = await client.GetAsync("tags");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };

                var tags = JsonSerializer.Deserialize<List<TagViewModel>>(content, options);
                return View(tags ?? new List<TagViewModel>());
            }

            return View(new List<TagViewModel>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tags");
            return View(new List<TagViewModel>());
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class QuestionViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorName { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QuestionCount { get; set; }
}

public class ErrorViewModel
{
    public string RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

