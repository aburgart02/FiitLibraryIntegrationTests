using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kontur.BigLibrary.DataAccess.Context
{
    public class BigLibraryContext : IdentityDbContext
    {
        public BigLibraryContext(DbContextOptions<BigLibraryContext> options)
            : base(options)
        {
        }
    }
}