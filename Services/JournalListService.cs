using Microsoft.Data.Sqlite;
using Coursework.Models;

namespace Coursework.Services
{
    public class JournalListService
    {
        private readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        public List<JournalEntry> GetAllEntries()
        {
            var entries = new List<JournalEntry>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                "SELECT Id, EntryDate, Content, PrimaryMood, CreatedAt, UpdatedAt FROM JournalEntries ORDER BY EntryDate DESC";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                entries.Add(new JournalEntry
                {
                    Id = reader.GetInt32(0),
                    EntryDate = DateTime.Parse(reader.GetString(1)),
                    Content = reader.GetString(2),
                    PrimaryMood = reader.GetString(3),
                    CreatedAt = DateTime.Parse(reader.GetString(4)),
                    UpdatedAt = DateTime.Parse(reader.GetString(5))
                });
            }

            return entries;
        }
    }
}
