using System.Data.SQLite;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Configuration;

namespace Kontur.BigLibrary.Tests.Core.Helpers
{
    public static class DbHelper
    {
        private const string TestDbName = "biglibrary-test.db";
        public const string ConnectionString = @"Data Source=./" + TestDbName;
        public static IDbConnectionFactory CreateConnectionFactory()
        {
            return new DbConnectionFactory(ConnectionString);
        }

        public static async Task CreateDbAsync()
        {
            DataAccessConfiguration.Configure();
            await DropDbAsync();
            await CreateDbFile();

            var builder = new SQLiteConnectionStringBuilder(ConnectionString);
            await using var connection = new SQLiteConnection(builder.ToString());
            await connection.OpenAsync();
            connection.LoadFtsExtension();
        }

        private static Task CreateDbFile()
        {
            File.Copy("biglibrary.db", TestDbName, true);
            return Task.CompletedTask;
        }

        public static async Task DropDbAsync()
        {
            File.Delete(TestDbName);
        }
    }
}