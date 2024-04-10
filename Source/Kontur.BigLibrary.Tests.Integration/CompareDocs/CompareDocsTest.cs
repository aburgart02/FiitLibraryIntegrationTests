using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Tests.Core.Helpers;
using Kontur.BigLibrary.Tests.Integration.BookServiceTests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.CompareDocs;

public class CompareDocsTest
{
    private static readonly IServiceProvider container = new ContainerWithRealBd().Build();
    private static readonly IBookService bookService = container.GetRequiredService<IBookService>();
    private static readonly IImageService imageService = container.GetRequiredService<IImageService>();
    private static readonly IBookRepository bookRepository = container.GetRequiredService<IBookRepository>();
    private int imageId;
    private const int BooksCount = 5;

    [OneTimeSetUp]
    public void SetUp()
    {
        var image = imageService
            .SaveAsync(new Image { Id = 1, Data = Array.Empty<byte>() }, new CancellationToken())
            .GetAwaiter().GetResult();
        imageId = image.Id!.Value;
    }

    [SetUp]
    public async Task CleanBooks()
    {
        var books = await bookRepository.SelectBooksAsync(new BookFilter(), CancellationToken.None);
        foreach (var book in books)
        {
            await bookRepository.DeleteBookAsync(book.Id!.Value, CancellationToken.None);
            await bookRepository.DeleteBookIndexAsync(book.Id!.Value, CancellationToken.None);
        }
    }

    [Test]
    public async Task Should_Have_ExpectedOrder_WithLINQ()
    {
        var books = await SaveBooks();
        
        var xmlResult =
            await bookService.ExportBooksToXmlAsync(new BookFilter() {Order = BookOrder.ByLastAdding} , CancellationToken.None);
        var xDoc = XDocument.Parse(xmlResult);
        
        for (var i = 0; i < BooksCount; i++)
        {
            xDoc.Descendants("Book")
                .ElementAt(i)
                .Element("Title")
                ?.Value.Should().Be(books[i].Name);
        }
    }
    
    [Test]
    public async Task Should_Have_ExpectedOrder_WithRegExp()
    {
        var books = await SaveBooks();
        
        var xmlResult =
            await bookService.ExportBooksToXmlAsync(new BookFilter() {Order = BookOrder.ByLastAdding} , CancellationToken.None);

        var regex = new Regex(@"<Title>(.*?)<\/Title>");
        var matches = regex.Matches(xmlResult);
        
        books.Select(x => x.Name).Should().BeEquivalentTo(matches.Select(x => x.Groups[1].Value));
    }
    
    [Test]
    public async Task Should_Have_ExpectedOrder_WithXDocumentCreation()
    {
        var books = await SaveBooks();
        
        var exportTime = DateTime.Now;
        var xmlResult =
            await bookService.ExportBooksToXmlAsync(new BookFilter() {Order = BookOrder.ByLastAdding} , CancellationToken.None);
        var xDoc = XDocument.Parse(xmlResult);
        
        var expDoc = new XDocument(
            new XElement("Books",
                new XElement("ExportTime", exportTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Book",
                    new XElement("Title", books[0].Name),
                    new XElement("Author", books[0].Author),
                    new XElement("Description", books[0].Description),
                    new XElement("RubricId", books[0].RubricId),
                    new XElement("ImageId", books[0].ImageId.ToString()),
                    new XElement("Price", books[0].Price),
                    new XElement("IsBusy", "false")
                ),
                new XElement("Book",
                    new XElement("Title", books[1].Name),
                    new XElement("Author", books[1].Author),
                    new XElement("Description", books[1].Description),
                    new XElement("RubricId", books[1].RubricId),
                    new XElement("ImageId", books[1].ImageId.ToString()),
                    new XElement("Price", books[1].Price),
                    new XElement("IsBusy", "false")
                ),
                new XElement("Book",
                    new XElement("Title", books[2].Name),
                    new XElement("Author", books[2].Author),
                    new XElement("Description", books[2].Description),
                    new XElement("RubricId", books[2].RubricId),
                    new XElement("ImageId", books[2].ImageId.ToString()),
                    new XElement("Price", books[2].Price),
                    new XElement("IsBusy", "false")
                ),
                new XElement("Book",
                    new XElement("Title", books[3].Name),
                    new XElement("Author", books[3].Author),
                    new XElement("Description", books[3].Description),
                    new XElement("RubricId", books[3].RubricId),
                    new XElement("ImageId", books[3].ImageId.ToString()),
                    new XElement("Price", books[3].Price),
                    new XElement("IsBusy", "false")
                ),
                new XElement("Book",
                    new XElement("Title", books[4].Name),
                    new XElement("Author", books[4].Author),
                    new XElement("Description", books[4].Description),
                    new XElement("RubricId", books[4].RubricId),
                    new XElement("ImageId", books[4].ImageId.ToString()),
                    new XElement("Price", books[4].Price),
                    new XElement("IsBusy", "false")
                )
            )
        );

        xDoc.Should().BeEquivalentTo(expDoc);
    }

    private static async Task<List<Book>> SaveBooks()
    {
        var books = new List<Book>();
        for (var i = 0; i < BooksCount; i++)
        {
            await imageService
                .SaveAsync(new Image { Id = i, Data = Array.Empty<byte>() }, new CancellationToken())
                .ConfigureAwait(false);
            var book = await bookService.SaveBookAsync(
                new BookBuilder().WithId(i).WithName($"Default name {i}").WithAuthor($"Default author {i}").WithImage(i)
                    .Build(), CancellationToken.None);
            books.Add(book);
        }
        books.Reverse();
        
        return books;
    }
}