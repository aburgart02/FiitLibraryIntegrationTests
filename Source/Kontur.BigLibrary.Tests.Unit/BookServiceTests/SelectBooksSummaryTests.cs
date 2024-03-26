using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SelectBooksSummaryTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldReturnNull_IfBookNotFound()
    {
        var bookFilter = new BookFilter();
        BookRepository.SelectBooksSummaryAsync(bookFilter, Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await BookService.SelectBooksSummaryAsync(bookFilter, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ShouldReturnBook_IfBookExist()
    {
        var bookFilter = new BookFilter();

        var bookSummary = new List<BookSummary> { GenerateBookSummary(1) };
        BookRepository.SelectBooksSummaryAsync(bookFilter, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<BookSummary>>(bookSummary));

        var result = await BookService.SelectBooksSummaryAsync(bookFilter, CancellationToken.None);

        Assert.That(result, Is.EqualTo(bookSummary));
    }
}