using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.BookService
{
    public interface IBookService
    {
        Task<Book> GetBookAsync(int id, CancellationToken cancellation);
        Task<Book> GetBookBySynonymAsync(string synonym, CancellationToken cancellation);
        Task<BookSummary> GetBookSummaryBySynonymAsync(string synonym, CancellationToken cancellation);
        Task<IReadOnlyList<BookSummary>> SelectBooksSummaryAsync(BookFilter filter, CancellationToken cancellation);
        Task<Book> SaveBookAsync(Book book, CancellationToken cancellation);
        Task<IReadOnlyList<Reader>> SaveReadersAsync(int bookId, Reader[] readers, CancellationToken cancellation);
        Task<IReadOnlyList<ReaderInQueue>> SaveReadersInQueueAsync(int bookId, ReaderInQueue[] readers, CancellationToken cancellation);
        Task<string> ExportBooksToXmlAsync(BookFilter filter, CancellationToken cancellation);
        Task<IReadOnlyList<Reader>> SelectReadersAsync(int bookId, CancellationToken cancellation);
        Task<IReadOnlyList<ReaderInQueue>> SelectReadersInQueueAsync(int bookId, CancellationToken cancellation);
        Task<string> CheckoutBookAsync(int bookId, string userName);
        Task<string> EnqueueAsync(int bookId, string userName);
        Task<string> ReturnBookAsync(int bookId, string userName);
    }
}