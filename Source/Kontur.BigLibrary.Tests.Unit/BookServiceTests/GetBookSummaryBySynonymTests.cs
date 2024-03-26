using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class GetBookSummaryBySynonymTests : BookServiceTestBase
{
    [Test]
    public async Task ShouldReturnNull_IfBookNotFound()
    {
        const string notExistingSynonym = "notExisting";
        BookRepository.GetBookSummaryBySynonymAsync(notExistingSynonym, Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await BookService.GetBookSummaryBySynonymAsync(notExistingSynonym, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ShouldReturnBook_IfBookExist()
    {
        const string existingSynonym = "existing";
        var generateBookSummary = GenerateBookSummary(1, existingSynonym);
        BookRepository.GetBookSummaryBySynonymAsync(existingSynonym, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(generateBookSummary));

        var result = await BookService.GetBookSummaryBySynonymAsync(existingSynonym, CancellationToken.None);

        Assert.That(result, Is.EqualTo(generateBookSummary));
    }
}