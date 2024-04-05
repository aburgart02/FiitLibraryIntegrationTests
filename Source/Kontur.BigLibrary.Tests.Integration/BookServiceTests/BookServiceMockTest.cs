using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.BookServiceTests;

public class BookServiceMockTest
{
    private const string Name = "Database Systems. The Complete Book";
    private const string Author = "Hector Garcia-Molina, Jeffrey D.Ullman, Jennifer Widom";
    private const string Description = "New_book";

    [Test]
    public async Task SaveBookAsync_ReturnSameBook_WhenSaveCorrectBook()
    {
        var container = new ContainerWithMocks().Build();
        var bookService = container.GetRequiredService<IBookService>();
        var bookRepository = container.GetRequiredService<IBookRepository>();
        var imageRepository = container.GetRequiredService<IImageRepository>();
        var eventRepository = container.GetRequiredService<IEventRepository>();
        
        var correctBook = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        imageRepository?.GetAsync(correctBook.ImageId,Arg.Any<CancellationToken>()).Returns(new Image());
        bookRepository?.GetRubricAsync(correctBook.RubricId, CancellationToken.None).Returns(new Rubric());
        bookRepository?.SaveBookAsync(correctBook, CancellationToken.None).Returns(correctBook);
        
        var result = await bookService.SaveBookAsync(correctBook, CancellationToken.None);
        result.Should().BeEquivalentTo(correctBook);
        await bookRepository.Received(1)?.SaveBookAsync(correctBook, CancellationToken.None)!;
        await bookRepository.Received(1)?.SaveBookIndexAsync(Arg.Any<int>(), correctBook.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await eventRepository.Received(1)?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRequiredFieldIsNull()
    {
        var container = new ContainerWithMocks().Build();
        var bookService = container.GetRequiredService<IBookService>();
        var bookRepository = container.GetRequiredService<IBookRepository>();
        var imageRepository = container.GetRequiredService<IImageRepository>();
        var eventRepository = container.GetRequiredService<IEventRepository>();
        
        var bookWithNullRequiredField = new Book()
        {
            Name = null,
            Author = Author,
            Description = Description
        };
        
        imageRepository?.GetAsync(bookWithNullRequiredField.ImageId, Arg.Any<CancellationToken>())
            .Returns(new Image());
        bookRepository?.GetRubricAsync(bookWithNullRequiredField.RubricId, CancellationToken.None)
            .Returns(new Rubric());
        bookRepository?.SaveBookAsync(bookWithNullRequiredField, CancellationToken.None)
            .Returns(bookWithNullRequiredField);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService
            .SaveBookAsync(bookWithNullRequiredField, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Не заполнены обязательные поля"));
        await bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNullRequiredField, CancellationToken.None)!;
        await bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNullRequiredField.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRubricIsNotExisting()
    {
        var container = new ContainerWithMocks().Build();
        var bookService = container.GetRequiredService<IBookService>();
        var bookRepository = container.GetRequiredService<IBookRepository>();
        var imageRepository = container.GetRequiredService<IImageRepository>();
        var eventRepository = container.GetRequiredService<IEventRepository>();
        
        var bookWithNotExistingRubric = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        imageRepository?.GetAsync(bookWithNotExistingRubric.ImageId,Arg.Any<CancellationToken>())
            .Returns(new Image());
        bookRepository?.GetRubricAsync(bookWithNotExistingRubric.RubricId, CancellationToken.None)
            .Returns((Rubric)null);
        bookRepository?.SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None)
            .Returns(bookWithNotExistingRubric);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService
            .SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая рубрика."));
        await bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None)!;
        await bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNotExistingRubric.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenImageIsNotExisting()
    {
        var container = new ContainerWithMocks().Build();
        var bookService = container.GetRequiredService<IBookService>();
        var bookRepository = container.GetRequiredService<IBookRepository>();
        var imageRepository = container.GetRequiredService<IImageRepository>();
        var eventRepository = container.GetRequiredService<IEventRepository>();
        
        var bookWithNotExistingImage = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        imageRepository?.GetAsync(bookWithNotExistingImage.ImageId,Arg.Any<CancellationToken>())
            .Returns((Image)null);
        bookRepository?.GetRubricAsync(bookWithNotExistingImage.RubricId, CancellationToken.None)
            .Returns(new Rubric());
        bookRepository?.SaveBookAsync(bookWithNotExistingImage, CancellationToken.None)
            .Returns(bookWithNotExistingImage);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService
            .SaveBookAsync(bookWithNotExistingImage, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая картинка."));
        await bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNotExistingImage, CancellationToken.None)!;
        await bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNotExistingImage.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
}