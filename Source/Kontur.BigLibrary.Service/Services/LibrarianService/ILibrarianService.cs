using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.LibrarianService;

public interface ILibrarianService
{
    Task<Librarian> GetLibrarianAsync(int id, CancellationToken cancellation);
    Task<IReadOnlyList<Librarian>> SelectLibrariansAsync(CancellationToken cancellation);
    Task<Librarian> SaveLibrarianAsync(Librarian librarian, CancellationToken cancellation);
    Task DeleteLibrarianAsync(int id, CancellationToken cancellation);
}