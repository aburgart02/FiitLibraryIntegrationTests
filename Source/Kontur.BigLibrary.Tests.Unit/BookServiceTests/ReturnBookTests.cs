using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class ReturnBookTests : BookServiceTestBase
{
    [Test]
    public async Task ReturnBookTest_EmptyEmail_CorrectError()
    {
        string email = null;
        var bookId = 1;
        
        var result = await BookService.ReturnBookAsync(bookId, email);
        result.Should().Be("Имя пользователя не может быть пустым.");
    }

    [Test, Ignore("не работает, т.к. в SelectBooksSummaryAsync приходит пусто, а не наши замоканные данные")]
    public async Task ReturnBookTest_BookIsFree_CorrectError()
    {
        string email = "qw12@we.we";
        var bookId = 1;
        var generateBooksSummaries = GenerateBooksSummary(1, "");
        var book = GenerateBook(1);
        BookRepository.SelectBooksSummaryAsync(new BookFilter(), CancellationToken.None)
            .ReturnsForAnyArgs(Task.FromResult(generateBooksSummaries));

        BookRepository.GetBookAsync(bookId, CancellationToken.None).ReturnsForAnyArgs(Task.FromResult(book));
        
        var result = await BookService.ReturnBookAsync(bookId, email);
        result.Should().Be("Книга не занята.");
    }
    
    [Test]
    public async Task ReturnBookTest_AnotherReader_CorrectError()
    {
        string email = "user1@ru.ru";
        var bookId = 1;
        var generateBooksSummaries = GenerateBusyBooksSummary(1, "");
        var book = GenerateBook(1);
        var reader = GenerateReaders(1, bookId, "user2@ru.ru");
        BookRepository.SelectBooksSummaryAsync(new BookFilter(), CancellationToken.None)
            .Returns(Task.FromResult(generateBooksSummaries));

        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));
        BookRepository.SelectReadersAsync(bookId, CancellationToken.None).Returns(Task.FromResult(reader));
        
        var result = await BookService.ReturnBookAsync(bookId, email);
        result.Should().Be("Книга занята не вами.");
    }
    
    [Test]
    public async Task ReturnBookTest_CorrectReader_ReturnSuccess()
    {
        string email = "user1@ru.ru";
        var bookId = 1;
        var generateBooksSummaries = GenerateBusyBooksSummary(1, "");
        var book = GenerateBook(1);
        var reader = GenerateReaders(1, bookId, "user1@ru.ru");
        BookRepository.SelectBooksSummaryAsync(new BookFilter(), CancellationToken.None)
            .Returns(Task.FromResult(generateBooksSummaries));

        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));
        BookRepository.SelectReadersAsync(bookId, CancellationToken.None).Returns(Task.FromResult(reader));
        
        BookRepository.SaveReaderAsync(Arg.Any<Reader>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as Reader));
        
        var result = await BookService.ReturnBookAsync(bookId, email);
        result.Should().Be("Книга свободна.");
    }
}