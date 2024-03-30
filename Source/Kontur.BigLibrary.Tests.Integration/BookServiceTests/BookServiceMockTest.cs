using System;
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
    private static IServiceProvider _container;
    private static IBookService _bookService;
    private static IBookRepository _bookRepository;
    private static IImageRepository _imageRepository;
    private static IEventRepository _eventRepository;
    
    private const string Name = "Database Systems. The Complete Book";
    private const string Author = "Hector Garcia-Molina, Jeffrey D.Ullman, Jennifer Widom";
    private const string Description = "New_book";

    [SetUp]
    public void SetUp()
    {
        _container = new ContainerWithMocks().Build();
        _bookService = _container.GetRequiredService<IBookService>();
        _bookRepository = _container.GetRequiredService<IBookRepository>();
        _imageRepository = _container.GetRequiredService<IImageRepository>();
        _eventRepository = _container.GetRequiredService<IEventRepository>();
    }
    
    [Test]
    public async Task SaveBookAsync_ReturnSameBook_WhenSaveCorrectBook()
    {
        var correctBook = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        _imageRepository?.GetAsync(correctBook.ImageId,Arg.Any<CancellationToken>()).Returns(new Image());
        _bookRepository?.GetRubricAsync(correctBook.RubricId, CancellationToken.None).Returns(new Rubric());
        _bookRepository?.SaveBookAsync(correctBook, CancellationToken.None).Returns(correctBook);
        
        var result = await _bookService.SaveBookAsync(correctBook, CancellationToken.None);
        result.Should().BeEquivalentTo(correctBook);
        await _bookRepository.Received()?.SaveBookAsync(correctBook, CancellationToken.None)!;
        await _bookRepository.Received()?.SaveBookIndexAsync(Arg.Any<int>(), correctBook.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await _eventRepository.Received()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRequiredFieldIsNull()
    {
        var bookWithNullRequiredField = new Book()
        {
            Name = null,
            Author = Author,
            Description = Description
        };
        
        _imageRepository?.GetAsync(bookWithNullRequiredField.ImageId, Arg.Any<CancellationToken>())
            .Returns(new Image());
        _bookRepository?.GetRubricAsync(bookWithNullRequiredField.RubricId, CancellationToken.None)
            .Returns(new Rubric());
        _bookRepository?.SaveBookAsync(bookWithNullRequiredField, CancellationToken.None)
            .Returns(bookWithNullRequiredField);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => _bookService
            .SaveBookAsync(bookWithNullRequiredField, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Не заполнены обязательные поля"));
        await _bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNullRequiredField, CancellationToken.None)!;
        await _bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNullRequiredField.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await _eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRubricIsNotExisting()
    {
        var bookWithNotExistingRubric = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        _imageRepository?.GetAsync(bookWithNotExistingRubric.ImageId,Arg.Any<CancellationToken>())
            .Returns(new Image());
        _bookRepository?.GetRubricAsync(bookWithNotExistingRubric.RubricId, CancellationToken.None)
            .Returns((Rubric)null);
        _bookRepository?.SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None)
            .Returns(bookWithNotExistingRubric);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => _bookService
            .SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая рубрика."));
        await _bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNotExistingRubric, CancellationToken.None)!;
        await _bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNotExistingRubric.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await _eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenImageIsNotExisting()
    {
        var bookWithNotExistingImage = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description
        };
        
        _imageRepository?.GetAsync(bookWithNotExistingImage.ImageId,Arg.Any<CancellationToken>())
            .Returns((Image)null);
        _bookRepository?.GetRubricAsync(bookWithNotExistingImage.RubricId, CancellationToken.None)
            .Returns(new Rubric());
        _bookRepository?.SaveBookAsync(bookWithNotExistingImage, CancellationToken.None)
            .Returns(bookWithNotExistingImage);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => _bookService
            .SaveBookAsync(bookWithNotExistingImage, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая картинка."));
        await _bookRepository.DidNotReceive()?.SaveBookAsync(bookWithNotExistingImage, CancellationToken.None)!;
        await _bookRepository.DidNotReceive()?.SaveBookIndexAsync(Arg.Any<int>(), bookWithNotExistingImage.GetTextForFts(),
            Arg.Any<string>(), CancellationToken.None)!;
        await _eventRepository.DidNotReceive()?.SaveAsync(Arg.Any<ChangedEvent>(), CancellationToken.None)!;
    }
}