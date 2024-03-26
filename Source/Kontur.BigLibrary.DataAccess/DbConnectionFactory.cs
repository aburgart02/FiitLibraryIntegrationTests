using System;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.BigLibrary.DataAccess
{
        public class DbConnectionFactory : IDbConnectionFactory
        {
            private readonly SQLiteConnectionStringBuilder connectionStringBuilder;

            public DbConnectionFactory(string connectionString)
            {
                this.connectionStringBuilder = new SQLiteConnectionStringBuilder(connectionString);
            }

            public async Task<IDbConnection> OpenAsync(CancellationToken cancellation)
            {
                var sqliteConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
                await sqliteConnection.OpenAsync(cancellation);
                sqliteConnection.LoadFtsExtension();
                return sqliteConnection;
            }
        }
}