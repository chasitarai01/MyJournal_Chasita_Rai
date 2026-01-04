using Microsoft.Data.Sqlite;
using Coursework.Models;

namespace Coursework.Services
{
    public class JournalCalendarService
    {
        private readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        public JournalEntry? GetEntryByDate(DateTime date)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                "SELECT * FROM JournalEntries WHERE EntryDate = @date";
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new JournalEntry
            {
                Id = reader.GetInt32(0),
                EntryDate = DateTime.Parse(reader.GetString(1)),
                Content = reader.GetString(2),
                PrimaryMood = reader.GetString(3),
                SecondaryMood1 = reader.IsDBNull(4) ? null : reader.GetString(4),
                SecondaryMood2 = reader.IsDBNull(5) ? null : reader.GetString(5),
                Tags = reader.IsDBNull(6) ? "" : reader.GetString(6),
                CreatedAt = DateTime.Parse(reader.GetString(7)),
                UpdatedAt = DateTime.Parse(reader.GetString(8))
            };
        }
    }
}
