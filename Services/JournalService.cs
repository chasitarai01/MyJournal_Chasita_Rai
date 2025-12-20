using Microsoft.Data.Sqlite;
using Coursework.Models;

namespace Coursework.Services
{
    public class JournalService
    {
        private readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        public JournalEntry? GetTodayEntry()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                "SELECT * FROM JournalEntries WHERE EntryDate = @date";
            cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new JournalEntry
            {
                Id = reader.GetInt32(0),
                EntryDate = DateTime.Parse(reader.GetString(1)),
                Content = reader.GetString(2),
                PrimaryMood = reader.GetString(3),
                SecondaryMood1 = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = DateTime.Parse(reader.GetString(7)),
                UpdatedAt = DateTime.Parse(reader.GetString(8))
            };

        }

        public void Save(JournalEntry entry)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            INSERT OR REPLACE INTO JournalEntries
            (EntryDate, Content, PrimaryMood, SecondaryMood1, SecondaryMood2, Tags, CreatedAt, UpdatedAt)
            VALUES (@date, @content, @pm, @sm1, @created, @updated)
            """;

            cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@content", entry.Content);
            cmd.Parameters.AddWithValue("@pm", entry.PrimaryMood);
            cmd.Parameters.AddWithValue("@sm1", entry.SecondaryMood1);
            cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@updated", DateTime.Now.ToString());
            



            cmd.ExecuteNonQuery();
        }
        

    }
}
