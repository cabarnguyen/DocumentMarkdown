using DocumentMarkdown.Models;
using DocumentMarkdown.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentMarkdown.Controllers
{
    public class MarkdownController : Controller
    {

        private readonly MarkdownService _markdownService;
        private readonly CsvService _csvService;
        private readonly string _markdownPath = Path.Combine("wwwroot", "markdown");

        public MarkdownController(MarkdownService markdownService, CsvService csvService)
        {
            _markdownService = markdownService;
            _csvService = csvService;
        }


        [HttpGet]
        public IActionResult Index()
        {

            var metadata = _csvService.ReadMetadata();
            ViewBag.Files = metadata;
            return View();
        }



        [HttpGet]
        public IActionResult ViewFile(string fileName)
        {
            var filePath = Path.Combine(_markdownPath, fileName);
            var markdownText = System.IO.File.ReadAllText(filePath);
            var htmlContent = _markdownService.ConvertToHtml(markdownText);

            var metadata = _csvService.ReadMetadata().FirstOrDefault(m => m.FilePath == fileName);

            ViewBag.FileName = fileName;
            ViewBag.MarkdownText = markdownText;
            ViewBag.HtmlContent = htmlContent;
            ViewBag.Title = metadata?.Title;

            return View("ViewFile");
        }

       

        [HttpPost]
        public IActionResult Convert(string markdownText, string fileName, string title, DateTime publishOn, string category)
        {
            // Ensure filename ends with .md
            if (!fileName.EndsWith(".md"))
            {
                fileName += ".md";
            }

            // Save Markdown text to file
            var filePath = Path.Combine(_markdownPath, fileName);
            System.IO.File.WriteAllText(filePath, markdownText);

            // Read existing metadata
            var metadata = _csvService.ReadMetadata();

            // Update metadata or add new entry
            var existingFile = metadata.FirstOrDefault(m => m.FilePath == fileName);
            if (existingFile != null)
            {
                existingFile.Title = title;
                existingFile.PublishOn = publishOn;
                existingFile.Category = category;
            }
            else
            {
                metadata.Add(new MarkdownFileMetadata
                {
                    Title = title,
                    FilePath = fileName,
                    PublishOn = publishOn,
                    Category = category
                });
            }

            // Save metadata
            _csvService.WriteMetadata(metadata);

            // Convert Markdown to HTML for display
            var htmlContent = _markdownService.ConvertToHtml(markdownText);
            ViewBag.HtmlContent = htmlContent;
            ViewBag.Files = metadata;

            return View("Index");
        }


      

        [HttpGet]
        public IActionResult Edit(string fileName)
        {
            var filePath = Path.Combine(_markdownPath, fileName);
            var markdownText = System.IO.File.ReadAllText(filePath);

            var metadata = _csvService.ReadMetadata().FirstOrDefault(m => m.FilePath == fileName);
            ViewBag.FileName = fileName;
            ViewBag.MarkdownText = markdownText;
            ViewBag.Title = metadata?.Title;
            ViewBag.PublishOn = metadata?.PublishOn;
            ViewBag.Category = metadata?.Category;

            return View("Edit");
        }

       

        [HttpPost]
        public IActionResult SaveEdit(string fileName, string markdownText, string title, DateTime publishOn, string category)
        {
            var filePath = Path.Combine(_markdownPath, fileName);
            System.IO.File.WriteAllText(filePath, markdownText);

            // Read existing metadata
            var metadata = _csvService.ReadMetadata();

            // Update metadata
            var existingFile = metadata.FirstOrDefault(m => m.FilePath == fileName);
            if (existingFile != null)
            {
                existingFile.Title = title;
                existingFile.PublishOn = publishOn;
                existingFile.Category = category;
            }

            // Save metadata
            _csvService.WriteMetadata(metadata);

            return RedirectToAction("ViewFile", new { fileName = fileName });
        }
    }
}
