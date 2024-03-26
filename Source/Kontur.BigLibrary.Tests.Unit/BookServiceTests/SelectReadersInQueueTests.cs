using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SelectReadersInQueueTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldReturnNull_IfBookNotFound()
    {
        var notExistedBookId = 12345;
        BookRepository.GetBookAsync(notExistedBookId, CancellationToken.None).ReturnsNull();
        BookRepository.SelectReadersAsync(notExistedBookId, Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await BookService.SelectReadersAsync(notExistedBookId, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ShouldReturnReaders_IfBookWithReaders()
    {
        const int bookId = 1;

        var expectedReaders = new List<ReaderInQueue> { GenerateReaderInQueue(1, bookId), GenerateReaderInQueue(2, bookId) };
        BookRepository.SelectReadersInQueueAsync(bookId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ReaderInQueue>>(expectedReaders));

        var result = await BookService.SelectReadersInQueueAsync(bookId, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedReaders));
    }
}