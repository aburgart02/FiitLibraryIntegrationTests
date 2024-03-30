using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.BookServiceTests;

public class BookServiceMockTest
{
    private static readonly IServiceProvider container = new ContainerWithMocks().Build();
    private static readonly IBookService bookService = container.GetRequiredService<IBookService>();
    private static readonly IImageService imageService = container.GetRequiredService<IImageService>();
    
    [Test]
    public async Task SaveBookAsync_ReturnSameBook_WhenSaveCorrectBook()
    {
        var imageForSave = new Image { Data = Array.Empty<byte>() };
        var startId = 1;
        
        var imageRepository = container.GetService<IImageRepository>();
        imageRepository?.SaveAsync(imageForSave, Arg.Any<CancellationToken>()).Returns(imageForSave);
        imageRepository?.GetAsync(startId + 1, Arg.Any<CancellationToken>()).Returns(imageForSave);
        
        var image = await imageService.SaveAsync(imageForSave, new CancellationToken()).ConfigureAwait(false);

        var book = new Book()
        {
            Name = "Database Systems. The Complete Book",
            Author = "Hector Garcia-Molina, Jeffrey D.Ullman, Jennifer Widom",
            RubricId = 1,
            ImageId = image.Id!.Value,
            Description = "New_book"
        };

        var bookRepository = container.GetService<IBookRepository>();
        bookRepository?.GetRubricAsync(book.RubricId, CancellationToken.None).Returns(new Rubric() {Id = book.Id});
        bookRepository?.SaveBookAsync(book, CancellationToken.None).Returns(book);
        
        var result = await bookService.SaveBookAsync(book, CancellationToken.None);

        result.Should().BeEquivalentTo(book);
    }
}