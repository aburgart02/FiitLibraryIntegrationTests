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
        
        var actualTitles = matches.Select(x => x.Groups[1].Value);
        var expectedTitles = books.Select(x => x.Name);
        Assert.That(actualTitles, Is.EqualTo(expectedTitles));
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
                new XElement("ExportTime", exportTime.ToString("yyyy-MM-dd HH:mm:ss"))
            )
        );
        for (var i = 0; i < 5; i++)
        {
            expDoc.Element("Books")?.Add(
                new XElement("Book",
                    new XElement("Title", books[i].Name),
                    new XElement("Author", books[i].Author),
                    new XElement("Description", books[i].Description),
                    new XElement("RubricId", books[i].RubricId),
                    new XElement("ImageId", books[i].ImageId.ToString()),
                    new XElement("Price", books[i].Price),
                    new XElement("IsBusy", "false")
                ));
        }

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