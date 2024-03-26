using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class GetBookTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldReturnNull_IfBookNotFound()
    {
        var bookId = 1;
        BookRepository.GetBookAsync(bookId, Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await BookService.GetBookAsync(bookId, CancellationToken.None);

        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task ShouldReturnBook_IfBookExist()
    {
        var bookId = 1;
        var expectedBook = GenerateBook(bookId);
        BookRepository.GetBookAsync(bookId, Arg.Any<CancellationToken>()).Returns(expectedBook);

        var result = await BookService.GetBookAsync(bookId, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedBook));
    }
}