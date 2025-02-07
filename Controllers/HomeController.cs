using DocumentMarkdown.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocumentMarkdown.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _markdownPath = Path.Combine("wwwroot", "markdown");
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var files = Directory.GetFiles(_markdownPath, "*.md")
                                 .Select(file => new FileInfo(file))
                                 .OrderByDescending(file => file.LastWriteTime)
                                 .Select(file => file.Name)
                                 .ToList();
            ViewBag.Files = files;
            return View();
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
}
