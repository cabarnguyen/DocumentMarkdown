using DocumentMarkdown.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentMarkdown.Components
{
    public class MarkdownFileListViewComponent : ViewComponent
    {
        private readonly string _markdownPath = Path.Combine("wwwroot", "markdown");
        private readonly CsvService _csvService;

        public MarkdownFileListViewComponent(CsvService csvService)
        {
            _csvService = csvService;
        }

        public IViewComponentResult Invoke()
        {
            var metadata = _csvService.ReadMetadata();
            var groupedFiles = metadata.GroupBy(m => m.Category).ToList();
            return View(groupedFiles);
            //var metadata = _csvService.ReadMetadata();
            //return View(metadata);
        }
    }
}
