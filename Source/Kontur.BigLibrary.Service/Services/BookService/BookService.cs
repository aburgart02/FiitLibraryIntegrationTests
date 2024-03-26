using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.ImageService;
using Microsoft.AspNetCore.Mvc;

namespace Kontur.BigLibrary.Service.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;
        private readonly IImageService imageService;
        private readonly IEventService eventService;
        private readonly ISynonymMaker synonymMaker;

        private readonly int startId = 0;

        public BookService(IBookRepository bookRepository, IImageService imageService, IEventService eventService,
            ISynonymMaker synonymMaker)
        {
            this.bookRepository = bookRepository;
            this.imageService = imageService;
            this.eventService = eventService;
            this.synonymMaker = synonymMaker;
        }

        public async Task<Book> GetBookAsync(int id, CancellationToken cancellation) =>
            await bookRepository.GetBookAsync(id, cancellation);

        public async Task<Book> GetBookBySynonymAsync(string synonym, CancellationToken cancellation) =>
            await bookRepository.GetBookBySynonymAsync(synonym, cancellation);

        public async Task<BookSummary> GetBookSummaryBySynonymAsync(string synonym, CancellationToken cancellation) =>
            await bookRepository.GetBookSummaryBySynonymAsync(synonym, cancellation);

        public async Task<IReadOnlyList<Book>> SelectBooksAsync(BookFilter filter, CancellationToken cancellation) =>
            await bookRepository.SelectBooksAsync(filter, cancellation);

        public async Task<string> ExportBooksToXmlAsync(BookFilter filter, CancellationToken cancellation) =>
            await bookRepository.ExportBooksToXmlAsync(filter, cancellation);

        public async Task<IReadOnlyList<Reader>> SelectReadersAsync(int bookId, CancellationToken cancellation) =>
            await bookRepository.SelectReadersAsync(bookId, cancellation);

        public async Task<IReadOnlyList<ReaderInQueue>> SelectReadersInQueueAsync(int bookId,
            CancellationToken cancellation) =>
            await bookRepository.SelectReadersInQueueAsync(bookId, cancellation);

        public async Task<IReadOnlyList<BookSummary>> SelectBooksSummaryAsync(BookFilter filter,
            CancellationToken cancellation) =>
            await bookRepository.SelectBooksSummaryAsync(filter, cancellation);

        public async Task<Book> SaveBookAsync(Book book, CancellationToken cancellation)
        {
            await ValidateBookAsync(book, cancellation);

            book.Id ??= await GetNextBookIdAsync(cancellation);
            var actualBook = await bookRepository.SaveBookAsync(book, cancellation);

            await SaveBookIndexAsync(actualBook, cancellation);
            await PublishEventAsync(actualBook, cancellation);
            return actualBook;
        }

        public async Task<IReadOnlyList<Reader>> SaveReadersAsync(int bookId, Reader[] readers,
            CancellationToken cancellation)
        {
            await ValidateReadersAsync(bookId, readers, cancellation);
            await DeleteReadersAsync(bookId, cancellation);

            var result = new List<Reader>();

            foreach (var reader in readers)
            {
                reader.Id ??= await GetNextReaderIdAsync(cancellation);
                var actualReader = await bookRepository.SaveReaderAsync(reader, cancellation);

                await PublishEventAsync(actualReader, cancellation);

                result.Add(actualReader);
            }

            return result;
        }

        public async Task<IReadOnlyList<ReaderInQueue>> SaveReadersInQueueAsync(int bookId, ReaderInQueue[] readers,
            CancellationToken cancellation)
        {
            await DeleteReadersInQueueAsync(bookId, cancellation);

            var result = new List<ReaderInQueue>();

            foreach (var reader in readers)
            {
                reader.Id ??= await GetNextReaderInQueueIdAsync(cancellation);
                var actualReader = await bookRepository.SaveReaderInQueueAsync(reader, cancellation);

                await PublishEventAsync(actualReader, cancellation);

                result.Add(actualReader);
            }

            return result;
        }


        private async Task<int> GetNextBookIdAsync(CancellationToken cancellation)
        {
            var maxId = await bookRepository.GetMaxBookIdAsync(cancellation);
            return (maxId ?? startId) + 1;
        }

        private async Task<int> GetNextReaderIdAsync(CancellationToken cancellation)
        {
            var maxId = await bookRepository.GetMaxReaderIdAsync(cancellation);
            return (maxId ?? startId) + 1;
        }

        private async Task<int> GetNextReaderInQueueIdAsync(CancellationToken cancellation)
        {
            var maxId = await bookRepository.GetMaxReaderInQueueIdAsync(cancellation);
            return (maxId ?? startId) + 1;
        }

        private async Task ValidateBookAsync(Book book, CancellationToken cancellation)
        {
            if (book.Name == null || book.Author == null || book.Author == null || book.Description == null)
            {
                throw new ValidationException("Не заполнены обязательные поля");   
            }

            var rubric = await bookRepository.GetRubricAsync(book.RubricId, cancellation);
            if (rubric == null)
            {
                throw new ValidationException("Указана несуществующая рубрика.");
            }

            var image = await imageService.GetAsync(book.ImageId, null, cancellation);
            if (image == null)
            {
                throw new ValidationException("Указана несуществующая картинка.");
            }
        }

        private async Task ValidateReadersAsync(int bookId, Reader[] readers, CancellationToken cancellation)
        {
            var book = await GetBookAsync(bookId, cancellation);

            if (book == null)
            {
                throw new ValidationException("Читать можно только существующие книги.");
            }

            if (book.Count < readers.Length)
            {
                throw new ValidationException("Читателей книг не может быть больше, чем самих книг.");
            }

            if (readers.Any(r => r.BookId != bookId))
            {
                throw new ValidationException("Некорректный запрос. Книги читателей не совпадают с запрашиваемой книгой");
            }
        }

        private async Task DeleteReadersAsync(int bookId, CancellationToken cancellation)
        {
            var readers = await SelectReadersAsync(bookId, cancellation);
            foreach (var reader in readers)
            {
                reader.IsDeleted = true;
                var actualReader = await bookRepository.SaveReaderAsync(reader, cancellation);

                await PublishEventAsync(actualReader, cancellation);
            }
        }

        private async Task DeleteReadersInQueueAsync(int bookId, CancellationToken cancellation)
        {
            var readers = await SelectReadersInQueueAsync(bookId, cancellation);
            foreach (var reader in readers)
            {
                reader.IsDeleted = true;
                var actualReader = await bookRepository.SaveReaderInQueueAsync(reader, cancellation);

                await PublishEventAsync(actualReader, cancellation);
            }
        }

        private async Task PublishEventAsync(Book book, CancellationToken cancellation)
        {
            var @event = book.CreateChangedEvent();
            await eventService.PublishEventAsync(@event, cancellation);
        }

        private async Task PublishEventAsync(Reader reader, CancellationToken cancellation)
        {
            var @event = reader.CreateChangedEvent();
            await eventService.PublishEventAsync(@event, cancellation);
        }

        private async Task PublishEventAsync(ReaderInQueue reader, CancellationToken cancellation)
        {
            var @event = reader.CreateChangedEvent();
            await eventService.PublishEventAsync(@event, cancellation);
        }

        private async Task SaveBookIndexAsync(Book book, CancellationToken cancellation)
        {
            var synonym = synonymMaker.Create(book.Name);
            var existsBook = await bookRepository.GetBookBySynonymAsync(synonym, cancellation);

            synonym = existsBook != null
                ? synonymMaker.Create($"{book.Name}_{book.Author}")
                : synonym;

            await bookRepository.SaveBookIndexAsync(book.Id.Value, book.GetTextForFts(), synonym, cancellation);
        }

        public async Task<string> CheckoutBookAsync(int bookId, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "Имя пользователя не может быть пустым.";
            }

            var booksSummary = await GetBooksSummary();
            var book = await bookRepository.GetBookAsync(bookId, CancellationToken.None);
            var bookSummary = booksSummary.FirstOrDefault(bs => bs.Name == book.Name);

            var queue = await bookRepository.SelectReadersInQueueAsync(bookId, CancellationToken.None);

            if (queue.Count > 0 && IsBookFree(bookSummary))
            {
                if (queue.First().UserName == userName)
                {
                    await DequeueUser(bookId, userName, queue);
                    await CheckOutBook(bookSummary, bookId, userName);

                    return "Вы взяли книгу.";
                }

                return "Не ваша очередь.";
            }

            if (IsBookBusy(bookSummary) || await IsBookTakenByUserAsync(bookId, userName))
            {
                return "Книга занята.";
            }

            await CheckOutBook(bookSummary, bookId, userName);

            return "Вы взяли книгу.";
        }

        public async Task<string> ReturnBookAsync(int bookId, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "Имя пользователя не может быть пустым.";
            }

            var booksSummary = await GetBooksSummary();
            var book = await bookRepository.GetBookAsync(bookId, CancellationToken.None);
            var bookSummary = booksSummary.FirstOrDefault(bs => bs.Name == book.Name);

            if (IsBookFree(bookSummary))
            {
                return "Книга не занята.";
            }

            if (await IsBookTakenByUserAsync(bookId, userName))
            {
                var readers = await bookRepository.SelectReadersAsync(bookId, CancellationToken.None);
                var reader = readers.FirstOrDefault(r => r.UserName == userName);
                await ReturnBook(reader);
                return "Книга свободна.";
            }

            return "Книга занята не вами.";
        }

        public async Task<string> EnqueueAsync(int bookId, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "Имя пользователя не может быть пустым.";
            }

            var booksSummary = await GetBooksSummary();
            var book = await bookRepository.GetBookAsync(bookId, CancellationToken.None);
            var bookSummary = booksSummary.FirstOrDefault(bs => bs.Name == book.Name);

            if (await IsBookTakenByUserAsync(bookId, userName))
            {
                return "Вы уже взяли эту книгу.";
            }

            var queue = await bookRepository.SelectReadersInQueueAsync(bookId, CancellationToken.None);

            if (IsBookFree(bookSummary) && queue.Count == 0)
            {
                return "Книга свободна.";
            }

            if (IsUserInQueue(queue, userName))
            {
                return "Вы уже стоите в очереди.";
            }

            if (IsUserInOtherQueues(userName, booksSummary))
            {
                return "Вы стоите в других очередях";
            }

            await EnqueueUser(bookId, userName, queue);

            return "Вы встали в очередь.";
        }

        private async Task<IEnumerable<BookSummary>> GetBooksSummary()
        {
            return await bookRepository.SelectBooksSummaryAsync(new BookFilter(), CancellationToken.None);
        }

        private bool IsBookBusy(BookSummary bookSummary)
        {
            return bookSummary?.IsBusy == true;
        }

        private bool IsBookFree(BookSummary bookSummary)
        {
            return bookSummary?.IsBusy == false;
        }

        private async Task<bool> IsBookTakenByUserAsync(int bookId, string userName)
        {
            var readers = await bookRepository.SelectReadersAsync(bookId, CancellationToken.None);
            return readers.Any(r => r.UserName == userName);
        }

        private async Task CheckOutBook(BookSummary bookSummary, int bookId, string userName)
        {
            bookSummary.IsBusy = true;
            var id = await bookRepository.GetMaxReaderIdAsync(CancellationToken.None) + 1;
            var actualReader = await bookRepository.SaveReaderAsync(new Reader()
            {
                BookId = bookId,
                UserName = userName,
                StartDate = DateTime.Now,
                Id = id
            }, CancellationToken.None);
            await PublishEventAsync(actualReader, CancellationToken.None);
        }

        private async Task ReturnBook(Reader reader)
        {
            reader.IsDeleted = true;
            var actualReader = await bookRepository.SaveReaderAsync(reader, CancellationToken.None);
            await PublishEventAsync(actualReader, CancellationToken.None);
        }

        private bool IsUserInQueue(IEnumerable<ReaderInQueue> queue, string userName)
        {
            return queue.Any(r => r.UserName == userName);
        }

        private bool IsUserInOtherQueues(string userName, IEnumerable<BookSummary> booksSummary)
        {
            var maxQueueCount = 5;
            var queueCount = 0;

            foreach (var bookSummary in booksSummary)
            {
                var currentQueue = bookRepository.SelectReadersInQueueAsync(bookSummary.Id, CancellationToken.None).Result;
                if (IsUserInQueue(currentQueue, userName))
                {
                    queueCount++;
                }
            }

            return queueCount > maxQueueCount;
        }

        private async Task EnqueueUser(int bookId, string userName, IEnumerable<ReaderInQueue> queue)
        {
            var readersInQueue = queue.ToList();
            readersInQueue.Add(new ReaderInQueue()
            {
                BookId = bookId,
                StartDate = DateTime.Now,
                UserName = userName
            });

            await SaveReadersInQueueAsync(bookId, readersInQueue.ToArray(), CancellationToken.None);
        }

        private async Task DequeueUser(int bookId, string userName, IEnumerable<ReaderInQueue> queue)
        {
            List<ReaderInQueue> readersInQueue = new List<ReaderInQueue>();

            foreach (var readerInQueue in queue.ToList())
            {
                if (readerInQueue.UserName != userName)
                {
                    readersInQueue.Add(readerInQueue);
                }
            }

            await SaveReadersInQueueAsync(bookId, readersInQueue.ToArray(), CancellationToken.None);
        }
    }
}