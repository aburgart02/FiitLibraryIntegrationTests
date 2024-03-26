using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class GetBookBySynonymTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldReturnNull_IfBookNotFound()
    { 
        const string notExistingSynonym = "notExisting";
 
        BookRepository.GetBookBySynonymAsync(notExistingSynonym, Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await BookService.GetBookBySynonymAsync(notExistingSynonym, CancellationToken.None);

        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task ShouldReturnBook_IfBookExist()
    {
        const int bookId = 1;
        const string existingSynonym = "existing";
        var expectedBook = GenerateBook(bookId);
        BookRepository.GetBookBySynonymAsync(existingSynonym, Arg.Any<CancellationToken>()).Returns(expectedBook);

        var result = await BookService.GetBookBySynonymAsync(existingSynonym, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedBook));
    }
}