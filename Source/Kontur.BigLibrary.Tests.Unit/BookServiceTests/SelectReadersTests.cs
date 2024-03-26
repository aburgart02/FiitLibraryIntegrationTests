using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SelectReadersTests : BookServiceTestBase
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

        var expectedReaders = new List<Reader> { GenerateReader(1, bookId), GenerateReader(2, bookId) };
        BookRepository.SelectReadersAsync(bookId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<Reader>>(expectedReaders));

        var result = await BookService.SelectReadersAsync(bookId, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedReaders));
    }
}