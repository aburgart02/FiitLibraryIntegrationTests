using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.EventService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Kontur.BigLibrary.Service.Services.SynonimMaker;
using Kontur.BigLibrary.Tests.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.BookServiceTests;

public class BookServiceTest
{
    #region WithContainer

    private static readonly IServiceProvider container = new ContainerWithRealBd().Build();
    private static readonly IBookService bookService = container.GetRequiredService<IBookService>();
    private static readonly IImageService imageService = container.GetRequiredService<IImageService>();

    #endregion

    #region CreateWithConstructor

    private readonly IImageService imageServiceOld = new ImageService(
        new ImageRepository(new DbConnectionFactory(DbHelper.ConnectionString)),
        new ImageTransformer());

    private readonly IBookService bookServiceOld =
        new BookService(new BookRepository(new DbConnectionFactory(DbHelper.ConnectionString)),
            imageService,
            new EventService(new EventRepository(new DbConnectionFactory(DbHelper.ConnectionString))),
            new SynonymMaker());

    #endregion

    private const string Name = "Database Systems. The Complete Book";
    private const string Author = "Hector Garcia-Molina, Jeffrey D.Ullman, Jennifer Widom";
    private const string Description = "New_book";

    [Test]
    public async Task SaveBookAsync_ReturnSameBook_WhenSaveCorrectBook()
    {
        var imageForSave = new Image { Data = Array.Empty<byte>() };
        var image = await imageService.SaveAsync(imageForSave, new CancellationToken()).ConfigureAwait(false);

        var book = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description,
            RubricId = 1,
            ImageId = image.Id!.Value
        };

        var result = await bookService.SaveBookAsync(book, CancellationToken.None);

        result.Should().BeEquivalentTo(book);
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRequiredFieldIsNull()
    {
        var book = new Book()
        {
            Name = null,
            Author = Author,
            Description = Description
        };
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService.SaveBookAsync(book, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Не заполнены обязательные поля"));
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenRubricIsNotExisting()
    {
        var book = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description,
        };
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService.SaveBookAsync(book, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая рубрика."));
    }
    
    [Test]
    public async Task SaveBookAsync_ThrowValidationException_WhenImageIsNotExisting()
    {
        var book = new Book()
        {
            Name = Name,
            Author = Author,
            Description = Description,
            RubricId = 1
        };
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => bookService.SaveBookAsync(book, CancellationToken.None));
        Assert.That(exception.Message, Is.EqualTo("Указана несуществующая картинка."));
    }
}