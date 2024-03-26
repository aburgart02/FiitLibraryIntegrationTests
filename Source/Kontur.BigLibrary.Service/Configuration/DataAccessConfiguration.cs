using Dapper;
using Kontur.BigLibrary.Service.Configuration.Dapper;

namespace Kontur.BigLibrary.Service.Configuration
{
    public static class DataAccessConfiguration
    {
        public static void Configure()
        {
            SqlMapper.AddTypeHandler(new ContactArrayTypeHandler());
            SqlMapper.AddTypeHandler(new EntityTypeHandler());
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}