using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Exceptions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SaveReaderTests : BookServiceTestBase
{
    [Test]
    public void ShouldThrowValidationException_IfBookNotExist()
    {
        SetDefaultMockSettings();
        var notExistedBookId = 12345;
        BookRepository.GetBookAsync(notExistedBookId, CancellationToken.None).ReturnsNull();
        var readers = new[] { GenerateReader(null, 123), GenerateReader(null, 123) };

        var action = () => BookService.SaveReadersAsync(notExistedBookId, readers, CancellationToken.None);

        action.Should().Throw<ValidationException>().WithMessage("Читать можно только существующие книги.");
    }

    [Test]
    public void ShouldThrowValidationException_IfBooksCountLessThanReadersCount()
    {
        SetDefaultMockSettings();
        var bookId = 123456;
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(GenerateBook(bookId)));
        var readers = new[] { GenerateReader(null, bookId), GenerateReader(null, bookId) };

        var action = () => BookService.SaveReadersAsync(bookId, readers, CancellationToken.None);

        action?.Should().Throw<ValidationException>()
            .WithMessage("Читателей книг не может быть больше, чем самих книг.");
    }

    [Test]
    public void ShouldThrowValidationException_IfBooksReadersBookIdNotEqualToPassedBookId()
    {
        SetDefaultMockSettings();
        var bookId = 123456;
        var readerWrongBookId = 654321;
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(GenerateBook(bookId)));
        var reader = GenerateReader(null, readerWrongBookId);

        var action = () => BookService.SaveReadersAsync(bookId, new[] { reader }, CancellationToken.None);

        action.Should().Throw<ValidationException>()
            .WithMessage("Некорректный запрос. Книги читателей не совпадают с запрашиваемой книгой");
    }

    [Test]
    public async Task ShouldDeleteBookCurrentReaders()
    {
        SetDefaultMockSettings();
        var bookId = 123456;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));
        var bookCurrentReaders = new[] { GenerateReader(1, bookId), GenerateReader(2, bookId) };
        var readers = new[] { GenerateReader(null, bookId) };
        BookRepository.SelectReadersAsync(bookId, Arg.Any<CancellationToken>()).Returns(bookCurrentReaders);

        await BookService.SaveReadersAsync(bookId, readers, CancellationToken.None);


        foreach (var reader in bookCurrentReaders)
        {
            await BookRepository.Received().SaveReaderAsync(
                Arg.Is<Reader>(x => x.IsDeleted == true && x.Id == reader.Id),
                CancellationToken.None);
            await EventService.Received()
                .PublishEventAsync(Arg.Is<ChangedEvent>(x => x.Source == reader.CreateChangedEvent().Source),
                    CancellationToken.None);
        }
    }

    [Test]
    public async Task ShouldSaveNewUsersWithIncrementedMaxId_IfReaderWithoutId()
    {
        SetDefaultMockSettings();
        var maxReaderId = 12;
        var bookId = 123456;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));
        BookRepository.GetMaxReaderIdAsync(Arg.Any<CancellationToken>()).Returns(maxReaderId);
        var newReader = GenerateReader(null, bookId);

        await BookService.SaveReadersAsync(bookId, new[] { newReader }, CancellationToken.None);

        await BookRepository.Received().SaveReaderAsync(
            Arg.Is<Reader>(x => x.Id == maxReaderId + 1 && x.BookId == newReader.BookId),
            CancellationToken.None);
    }


    [Test]
    public async Task ShouldSaveUsersWithPassedId_IfReaderWithId()
    {
        SetDefaultMockSettings();
        var maxReaderId = 12;
        var bookId = 123456;
        var readerId = 123;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));

        var newReader = GenerateReader(readerId, bookId);
        await BookService.SaveReadersAsync(bookId, new[] { newReader }, CancellationToken.None);

        await BookRepository.Received().SaveReaderAsync(
            Arg.Is<Reader>(x => x.Id == readerId && x.Id == newReader.Id),
            CancellationToken.None);
    }


    [Test]
    public async Task ShouldReturnPassedReaders()
    {
        SetDefaultMockSettings();
        var maxReaderId = 12;
        var bookId = 123456;
        var readerId = 123;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));

        var newReaders = new[]{GenerateReader(123, bookId)};
        var result = await BookService.SaveReadersAsync(bookId, newReaders, CancellationToken.None);

      Assert.That(result, Is.EqualTo(newReaders));
    }


    private void SetDefaultMockSettings()
    {
        BookRepository.GetRubricAsync(Arg.Any<int>(), CancellationToken.None).Returns(Task.FromResult(new Rubric()));
        ImageService.GetAsync(Arg.Any<int>(), null, CancellationToken.None).Returns(Task.FromResult(new Image()));
        BookRepository.GetMaxBookIdAsync(CancellationToken.None).Returns(Task.FromResult<int?>(0));
        BookRepository.SaveReaderAsync(Arg.Any<Reader>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as Reader));
        BookRepository.SaveBookAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as Book));
        BookRepository.GetBookBySynonymAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ReturnsNullForAnyArgs();
    }
}