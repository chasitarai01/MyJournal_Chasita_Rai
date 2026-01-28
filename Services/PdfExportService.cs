using Microsoft.EntityFrameworkCore;
using MyJournal.Data;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MyJournal.Services
{
    public class PdfExportService
    {
        private readonly AppDbContext _db;
        public PdfExportService(AppDbContext db) => _db = db;

        public async Task<string> ExportAsync(DateTime from, DateTime to)
        {
            var fromKey = from.ToString("yyyy-MM-dd");
            var toKey = to.ToString("yyyy-MM-dd");

            var entries = await _db.JournalEntries
                .Where(e => string.Compare(e.EntryDate, fromKey) >= 0 && string.Compare(e.EntryDate, toKey) <= 0)
                .OrderBy(e => e.EntryDate)
                .ToListAsync();

            var outPath = Path.Combine(FileSystem.AppDataDirectory,
                $"MyJournal_{fromKey}_to_{toKey}.pdf");

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"MyJournal Export ({fromKey} → {toKey})").FontSize(18).Bold();
                        col.Item().Text($"Entries: {entries.Count}").FontSize(12);

                        foreach (var e in entries)
                        {
                            col.Item().PaddingTop(10).Text($"{e.EntryDate} - {e.Title}").Bold();
                            col.Item().Text($"Mood: {e.MoodCategory} / {e.PrimaryMood}");
                            col.Item().Text(e.Content ?? "");
                        }
                    });
                });
            });

            doc.GeneratePdf(outPath);
            return outPath;
        }
    }
}
