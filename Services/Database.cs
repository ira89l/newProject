using SQLite;
using CrossHealthX.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossHealthX.Services
{
    public class ActivityDatabase
    {
        private static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "activities.db3");
        private SQLiteAsyncConnection? _database;

        private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(DbPath);
                await _database.CreateTableAsync<Activity>();
            }
            return _database;
        }

        // Отримати один запис за Id
        public async Task<Activity?> GetActivityAsync(int id)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<Activity>()
                           .Where(a => a.Id == id)
                           .FirstOrDefaultAsync();
        }

        // Отримати всі записи
        public async Task<List<Activity>> GetActivitiesAsync()
        {
            var db = await GetDatabaseAsync();
            return await db.Table<Activity>().ToListAsync();
        }

        // Зберегти або додати новий запис
        public async Task<int> SaveActivityAsync(Activity activity)
        {
            var db = await GetDatabaseAsync();
            if (activity.Id != 0)
            {
                return await db.UpdateAsync(activity);
            }
            return await db.InsertAsync(activity);
        }

        // Видалити один запис
        public async Task<int> DeleteActivityAsync(Activity activity)
        {
            var db = await GetDatabaseAsync();
            return await db.DeleteAsync(activity);
        }

        // Видалити всі записи
        public async Task<int> DeleteAllActivitiesAsync()
        {
            var db = await GetDatabaseAsync();
            return await db.DeleteAllAsync<Activity>();
        }
    }
}
