using Cantina.SyncService.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cantina.SyncService
{
    public static class DB
    {
        public static async Task<List<MenuItem>> GetMenuItems(string connectionString)
        {
            using var db = new SqlConnection(connectionString);
            await db.OpenAsync();
            var items = await db.QueryAsync<MenuItem>("SELECT * FROM MenuItems WHERE IsDeleted = 0");
            return [.. items];
        }

        internal static async Task<List<Review>> GetReviews(string connectionString)
        {
            using var db = new SqlConnection(connectionString);
            await db.OpenAsync();
            var items = await db.QueryAsync<Review>("SELECT * FROM Reviews");
            return [.. items];
        }
    }
}
