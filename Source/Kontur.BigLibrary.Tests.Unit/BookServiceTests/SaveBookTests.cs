using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Services.BookService;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class SaveBookTests : BookServiceTestBase
{
    [Test]
    public void SaveBook_ShouldThrowValidationException_IfRubricNotExist()
    {
        var book = GenerateBook(null);
        BookRepository.GetRubricAsync(book.RubricId, CancellationToken.None).Returns(Task.FromResult<Rubric>(null));

        var action = () => BookService.SaveBookAsync(book, CancellationToken.None);

        action.Should().Throw<ValidationException>().WithMessage("Указана несуществующая рубрика.");
    }

    [Test]
    public void SaveBook_ShouldThrowValidationException_IfImageNotExist()
    {
        var book = GenerateBook(null);
        BookRepository.GetRubricAsync(book.RubricId, CancellationToken.None).Returns(Task.FromResult(new Rubric()));
        ImageService.GetAsync(book.ImageId, null, CancellationToken.None).Returns(Task.FromResult<Image>(null));

        var action = () => BookService.SaveBookAsync(book, CancellationToken.None);

        action.Should().Throw<ValidationException>().WithMessage("Указана несуществующая картинка.");
    }


    [Test]
    public async Task SaveBook_ShouldSaveBookWithIncrementedMaxId_IfBookHasNoId()
    {
        SetDefaultMockSettings();
        var book = GenerateBook(null);
        var currentMaxBookId = 800;

        var expectedNewBookId = currentMaxBookId + 1;
        BookRepository.GetMaxBookIdAsync(CancellationToken.None).Returns(Task.FromResult<int?>(currentMaxBookId));

        await BookService.SaveBookAsync(book, CancellationToken.None);


        await BookRepository.Received()
            .SaveBookAsync(Arg.Is<Book>(x => x.Id == expectedNewBookId), CancellationToken.None);
    }

    [Test]
    public async Task SaveBook_ShouldNotChangeBookId_IfBookHasId()
    {
        SetDefaultMockSettings();
        var book = GenerateBook(123456);

        await BookService.SaveBookAsync(book, CancellationToken.None);
        await BookRepository.Received()
            .SaveBookAsync(Arg.Is<Book>(x => x.Id == book.Id), CancellationToken.None);
    }

    [Test]
    public async Task SaveBook_ShouldPublishEvent()
    {
        SetDefaultMockSettings();
        var book = GenerateBook(null);

        book = await BookService.SaveBookAsync(book, CancellationToken.None);
        var expectedEvent = book.CreateChangedEvent();

        await EventService.Received()
            .PublishEventAsync(Arg.Is<ChangedEvent>(x => x.Source == expectedEvent.Source),
                CancellationToken.None);
    }


    [Test]
    public async Task SaveBook_ShouldSaveIndexWithBookNameSynonym_IfNoBookNameSynonym()
    {
        SetDefaultMockSettings();
        var book = GenerateBook(null);
        var expectedSynonym = SynonymMaker.Create(book.Name);

        book = await BookService.SaveBookAsync(book, CancellationToken.None);

        await BookRepository.Received()
            .SaveBookIndexAsync(book.Id.Value, book.GetTextForFts(), expectedSynonym, CancellationToken.None);
    }

    [Test]
    public async Task SaveBook_ShouldSaveIndexWithBookNameAndAuthorSynonym_IfBookNameSynonymExists()
    {
        SetDefaultMockSettings();
        var book = GenerateBook(null);
        BookRepository.GetBookBySynonymAsync(book.Name, Arg.Any<CancellationToken>()).Returns(GenerateBook(12312));
        SynonymMaker.Create(Arg.Any<string>()).Returns(x => x[0]);

        book = await BookService.SaveBookAsync(book, CancellationToken.None);


        var expectedSynonym = $"{book.Name}_{book.Author}";
        await BookRepository.Received()
            .SaveBookIndexAsync(book.Id.Value, book.GetTextForFts(), expectedSynonym, CancellationToken.None);
    }


    private void SetDefaultMockSettings()
    {
        BookRepository.GetRubricAsync(Arg.Any<int>(), CancellationToken.None).Returns(Task.FromResult(new Rubric()));
        ImageService.GetAsync(Arg.Any<int>(), null, CancellationToken.None).Returns(Task.FromResult(new Image()));
        BookRepository.GetMaxBookIdAsync(CancellationToken.None).Returns(Task.FromResult<int?>(0));
        BookRepository.SaveBookAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x[0] as Book));
        BookRepository.GetBookBySynonymAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ReturnsNullForAnyArgs();
    }
}