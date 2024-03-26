using System.Data.SQLite;

namespace Kontur.BigLibrary.DataAccess
{
    public static class SQLiteConnectionExtensions
    {
        public static void LoadFtsExtension(this SQLiteConnection connection)
        {
            connection.EnableExtensions(true);
            connection.LoadExtension("SQLite.Interop.dll", "sqlite3_fts5_init");
        }
    }
}