using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SaveReadersInQueueTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldDeleteBookCurrentReadersInQueue()
    {
        SetDefaultMockSettings();
        var bookId = 123456;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));
        var bookCurrentReadersInQueue = new List<ReaderInQueue>
            { GenerateReaderInQueue(1, bookId), GenerateReaderInQueue(2, bookId) };
        var newReadersInQueue = new[] { GenerateReaderInQueue(null, bookId) };
        var expectedReaders = new List<ReaderInQueue>
            { GenerateReaderInQueue(1, bookId), GenerateReaderInQueue(2, bookId) };

        BookRepository.SelectReadersInQueueAsync(bookId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ReaderInQueue>>(bookCurrentReadersInQueue));

        await BookService.SaveReadersInQueueAsync(bookId, newReadersInQueue, CancellationToken.None);


        foreach (var reader in bookCurrentReadersInQueue)
        {
            await BookRepository.Received().SaveReaderInQueueAsync(
                Arg.Is<ReaderInQueue>(x => x.IsDeleted == true && x.Id == reader.Id),
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
        BookRepository.GetMaxReaderInQueueIdAsync(Arg.Any<CancellationToken>()).Returns(maxReaderId);
        var newReader = GenerateReaderInQueue(null, bookId);

        await BookService.SaveReadersInQueueAsync(bookId, new[] { newReader }, CancellationToken.None);

        await BookRepository.Received().SaveReaderInQueueAsync(
            Arg.Is<ReaderInQueue>(x => x.Id == maxReaderId + 1 && x.BookId == newReader.BookId),
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

        var newReader = GenerateReaderInQueue(readerId, bookId);
        await BookService.SaveReadersInQueueAsync(bookId, new[] { newReader }, CancellationToken.None);

        await BookRepository.Received().SaveReaderInQueueAsync(
            Arg.Is<ReaderInQueue>(x => x.Id == readerId && x.Id == newReader.Id),
            CancellationToken.None);
    }


    [Test]
    public async Task ShouldReturnPassedReadersInQueue()
    {
        SetDefaultMockSettings();
        var maxReaderId = 12;
        var bookId = 123456;
        var readerId = 123;
        var book = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, CancellationToken.None).Returns(Task.FromResult(book));

        var newReaders = new[] { GenerateReaderInQueue(123, bookId), GenerateReaderInQueue(456, bookId) };
        var result = await BookService.SaveReadersInQueueAsync(bookId, newReaders, CancellationToken.None);

        Assert.That(result, Is.EqualTo(newReaders));
    }


    private void SetDefaultMockSettings()
    {
        BookRepository.GetRubricAsync(Arg.Any<int>(), CancellationToken.None).Returns(Task.FromResult(new Rubric()));
        ImageService.GetAsync(Arg.Any<int>(), null, CancellationToken.None).Returns(Task.FromResult(new Image()));
        BookRepository.GetMaxBookIdAsync(CancellationToken.None).Returns(Task.FromResult<int?>(0));
        BookRepository.SaveReaderInQueueAsync(Arg.Any<ReaderInQueue>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as ReaderInQueue));
        BookRepository.SaveBookAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as Book));
        BookRepository.GetBookBySynonymAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ReturnsNullForAnyArgs();
    }
}