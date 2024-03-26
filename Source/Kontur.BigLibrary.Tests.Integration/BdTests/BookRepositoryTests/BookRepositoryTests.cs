using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Tests.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.BdTests.BookRepositoryTests;

[Parallelizable(ParallelScope.All)]
public class BookRepositoryTests
{
    private readonly IBookRepository bookRepository;

    #region CleanUpAfterEveryTest

    private ConcurrentBag<int> bookIds = new();

    [TearDown]
    public async Task Teardown()
    {
        foreach (var id in bookIds)
        {
            await bookRepository.DeleteBookAsync(id, CancellationToken.None);
            await bookRepository.DeleteBookIndexAsync(id, CancellationToken.None);
        }
    }

    #endregion


    public BookRepositoryTests()
    {
        var container = new Container().Build();
        bookRepository = container.GetRequiredService<IBookRepository>();
    }

    [Test]
    public async Task GetBook_Exists_ReturnBook()
    {
        var expectedBook = new BookBuilder().Build();
        var book = await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            $"book {expectedBook.Id}", CancellationToken.None);
        bookIds.Add(expectedBook.Id.Value);

        var actualBook = await bookRepository.GetBookAsync(expectedBook.Id.Value, CancellationToken.None);

        actualBook.Should().BeEquivalentTo(expectedBook);
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public async Task GetMaxBookId_Exists_ReturnBooksCount()
    {
        await CleanBooks();
        var expectedBook = new BookBuilder().WithId(1).Build();
        await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            $"book {expectedBook.Id}", CancellationToken.None);

        var maxId = await bookRepository.GetMaxBookIdAsync(CancellationToken.None);

        maxId.Should().Be(expectedBook.Id);
    }

    public async Task CleanBooks()
    {
        var books = await bookRepository.SelectBooksAsync(new BookFilter(), CancellationToken.None);
        foreach (var book in books)
        {
            await bookRepository.DeleteBookAsync(book.Id!.Value, CancellationToken.None);
            await bookRepository.DeleteBookIndexAsync(book.Id!.Value, CancellationToken.None);
        }
    }
}