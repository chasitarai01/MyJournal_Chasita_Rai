using Microsoft.Data.Sqlite;

namespace Coursework.Data
{
    public class DatabaseService
    {
        private readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        public void Initialize()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
            """
            CREATE TABLE IF NOT EXISTS JournalEntries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EntryDate TEXT UNIQUE,
                Content TEXT,
                PrimaryMood TEXT NOT NULL,
                SecondaryMood1 TEXT,
                CreatedAt TEXT,
                UpdatedAt TEXT
            );
            """;

            cmd.ExecuteNonQuery();
        }
        public void ResetDatabase()
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }

            Initialize(); 
        }
    }
}
