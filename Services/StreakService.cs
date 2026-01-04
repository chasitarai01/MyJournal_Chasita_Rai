using Microsoft.Data.Sqlite;

namespace Coursework.Services
{
    public class StreakService
    {
        private readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "journal.db");

        public (int currentStreak, int longestStreak, int missedDays) CalculateStreaks()
        {
            var dates = new List<DateTime>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT EntryDate FROM JournalEntries ORDER BY EntryDate ASC";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dates.Add(DateTime.Parse(reader.GetString(0)));
            }

            if (dates.Count == 0)
                return (0, 0, 0);

            int currentStreak = 1;
            int longestStreak = 1;
            int tempStreak = 1;
            int missedDays = 0;

            for (int i = 1; i < dates.Count; i++)
            {
                var diff = (dates[i] - dates[i - 1]).Days;

                if (diff == 1)
                {
                    tempStreak++;
                    longestStreak = Math.Max(longestStreak, tempStreak);
                }
                else
                {
                    missedDays += diff - 1;
                    tempStreak = 1;
                }
            }

            if ((DateTime.Today - dates.Last()).Days == 1)
                currentStreak = tempStreak;
            else if ((DateTime.Today - dates.Last()).Days > 1)
                currentStreak = 0;
            else
                currentStreak = tempStreak;

            return (currentStreak, longestStreak, missedDays);
        }
    }
}
