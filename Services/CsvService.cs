using CsvHelper;
using DocumentMarkdown.Models;
using System.Globalization;

namespace DocumentMarkdown.Services
{
    public class CsvService
    {
        private readonly string _csvFilePath = Path.Combine("wwwroot", "markdown", "metadata.csv");
        public List<MarkdownFileMetadata> ReadMetadata()
        {
            if (!System.IO.File.Exists(_csvFilePath))
            {
                return new List<MarkdownFileMetadata>();
            }

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return new List<MarkdownFileMetadata>(csv.GetRecords<MarkdownFileMetadata>());
            }
        }

        public void WriteMetadata(List<MarkdownFileMetadata> metadata)
        {
            using (var writer = new StreamWriter(_csvFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(metadata);
            }
        }
    }
}
