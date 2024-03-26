using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.BigLibrary.DataAccess
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> OpenAsync(CancellationToken cancellation);
    }
}