using System;
using System.IO;
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

[NonParallelizable]
public class CompareDocsTestExample
{
    private static readonly IServiceProvider container = new ContainerWithRealBd().Build();
    private static readonly IBookService bookService = container.GetRequiredService<IBookService>();
    private static readonly IImageService imageService = container.GetRequiredService<IImageService>();
    private static readonly IBookRepository bookRepository = container.GetRequiredService<IBookRepository>();
    private int imageId;

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
    public async Task Should_NotBeEmpty()
    {
        var book = CreateBook(); //Сохранение книги в БД

        var xmlResult =
            await bookService.ExportBooksToXmlAsync(CreateFilter(), CancellationToken.None); //выгружаем книги

        xmlResult.Should().NotBeEmpty(); //проверяем, что выгруженные данные не пустые
    }

    [Test]
    public async Task Should_Have_ExpectedInfo_RussianName()
    {
        var book = Books.RussianBook;
        await bookService.SaveBookAsync(book, CancellationToken.None); //создание книги с русскоязычным названием

        var xmlResult =
            await bookService.ExportBooksToXmlAsync(new BookFilter(), CancellationToken.None); //выгрузка данных
        var xDoc = XDocument.Parse(xmlResult);

        xDoc.Should().HaveElement("Book")
            .Which.Should().HaveElement("Title")
            .Which.Should().HaveValue(book.Name);
    }

    [Test]
    public async Task Should_NotContainBook_When_NoData()
    {
        await CreateBook();

        var xmlResult = await bookService.ExportBooksToXmlAsync(CreateFilter(isBusy: true), CancellationToken.None);

        xmlResult.Should()
            .Contain("<Books>").And
            .Contain("<ExportTime>").And
            .NotContainAny("<Book>");
    }

    [Test]
    public async Task Should_Have_ExpectedCountOfBooks()
    {
        for (var i = 0; i < 5; i++)
        {
            await CreateBook();
        }

        var xmlResult = await bookService.ExportBooksToXmlAsync(CreateFilter(limit: 4), CancellationToken.None);
        var xDoc = XDocument.Parse(xmlResult);

        xDoc.Should().HaveElement("Book", Exactly.Times(4));

        var count = new Regex("<Book>").Matches(xmlResult).Count;
        count.Should().Be(4);
    }

    [Test]
    public async Task Should_Be_ExpectedXML()
    {
        var book = CreateBook().GetAwaiter().GetResult();

        var exportTime = DateTime.Now;
        var xmlResult = await bookService.ExportBooksToXmlAsync(CreateFilter(), CancellationToken.None);
        var xDoc = XDocument.Parse(xmlResult);

        var expDoc = new XDocument(
            new XElement("Books",
                new XElement("ExportTime", exportTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Book",
                    new XElement("Title", book.Name),
                    new XElement("Author", book.Author),
                    new XElement("Description", book.Description),
                    new XElement("RubricId", book.RubricId),
                    new XElement("ImageId", book.ImageId.ToString()),
                    new XElement("Price", book.Price),
                    new XElement("IsBusy", "false")
                )
            )
        );

        xDoc.Should().BeEquivalentTo(expDoc);
    }

    [Test]
    public async Task Should_Be_ExpectedXML_File()
    {
        for (var i = 0; i < 100; i++)
        {
            await imageService
                .SaveAsync(new Image { Id = i, Data = Array.Empty<byte>() }, new CancellationToken())
                .ConfigureAwait(false);
            await bookService.SaveBookAsync(
                new BookBuilder().WithId(i).WithName($"Default name {i}").WithAuthor($"Default author {i}").WithImage(i)
                    .Build(), CancellationToken.None);
        }

        var exportTime = DateTime.Now;
        var xmlResult = await bookService.ExportBooksToXmlAsync(CreateFilter(limit: 100), CancellationToken.None);
        var xDoc = XDocument.Parse(xmlResult);

        var expDoc = XDocument.Parse(
            File.ReadAllTextAsync(Path.Combine("Files", "exportBooks.xml")).GetAwaiter().GetResult()
                .Replace("*", exportTime.ToString("yyyy-MM-dd HH:mm:ss")));

        xDoc.Should().BeEquivalentTo(expDoc);
    }

    private async Task<Book> CreateBook()
    {
        var book = new BookBuilder().WithImage(imageId).Build(); //создание книги
        await bookService.SaveBookAsync(book, CancellationToken.None); //Сохранение книги в БД
        return book;
    }

    private BookFilter CreateFilter(string query = "", string rubric = "", int? limit = 10, bool isBusy = false,
        BookOrder order = BookOrder.ByLastAdding, int offset = 0)
    {
        return new()
        {
            Query = query,
            RubricSynonym = rubric,
            IsBusy = isBusy,
            Limit = limit,
            Order = order,
            Offset = offset
        };
    }
}