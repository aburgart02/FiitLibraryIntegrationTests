using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Tests.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration.BdTests.BookRepositoryTests;

public class BookRepositoryTests
{
    private readonly IBookRepository bookRepository;
    private ConcurrentBag<int> bookIds = new();

    [SetUp]
    public async Task CleanBooks()
    {
        foreach (var book in bookIds)
        {
            await bookRepository.DeleteBookAsync(book, CancellationToken.None);
            await bookRepository.DeleteBookIndexAsync(book, CancellationToken.None);
        }
    }

    public BookRepositoryTests()
    {
        var container = new Container().Build();
        bookRepository = container.GetRequiredService<IBookRepository>();
    }

    [Test]
    public async Task GetBookAsync_ReturnBook_ExistingBook()
    {
        var expectedBook = new BookBuilder().Build();
        var book = await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            $"book {expectedBook.Id}", CancellationToken.None);
        bookIds.Add(expectedBook.Id.Value);

        var actualBook = await bookRepository.GetBookAsync(expectedBook.Id.Value, CancellationToken.None);

        actualBook.Should().BeEquivalentTo(expectedBook);
    }
    
    [Test]
    public async Task GetBookAsync_ReturnBook_BookWithLongDescription()
    {
        var description = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10000000)
            .Select(s => s[IntGenerator.GetInRange(s.Length)]).ToArray());
        var expectedBook = new BookBuilder().WithDescription(description).Build();
        var book = await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            $"book {expectedBook.Id}", CancellationToken.None);
        bookIds.Add(expectedBook.Id.Value);

        var actualBook = await bookRepository.GetBookAsync(expectedBook.Id.Value, CancellationToken.None);

        actualBook.Should().BeEquivalentTo(expectedBook);
    }
    
    [Test]
    public async Task GetBookAsync_ReturnNull_NotExistingBook()
    {
        var actualBook = await bookRepository.GetBookAsync(-1, CancellationToken.None);
        actualBook.Should().BeEquivalentTo((Book)null);
    }
    
    [Test]
    public async Task SelectBooksAsync_ReturnBookByName_ExistingBook()
    {
        var expectedBookName = Guid.NewGuid().ToString();
        var expectedBook = new BookBuilder().WithName(expectedBookName).Build();
        await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            $"book {expectedBook.Id}", CancellationToken.None);
        bookIds.Add(expectedBook.Id.Value);
        
        var anotherBookName = Guid.NewGuid().ToString();
        var anotherBook = new BookBuilder().WithName(anotherBookName).Build();
        await bookRepository.SaveBookAsync(anotherBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(anotherBook.Id!.Value, anotherBook.GetTextForFts(),
            $"book {anotherBook.Id}", CancellationToken.None);
        bookIds.Add(anotherBook.Id.Value);

        var actualBook = await bookRepository.SelectBooksAsync(new BookFilter(){Query = expectedBookName}, CancellationToken.None);

        actualBook.FirstOrDefault().Should().BeEquivalentTo(expectedBook);
    }
    
    [Test]
    public async Task SelectBooksAsync_ReturnNull_DeletedBook()
    {
        var book = new BookBuilder().Build();
    
        var actualBook = await bookRepository.GetBookAsync(book.Id.Value, CancellationToken.None);
        actualBook.Should().BeEquivalentTo((Book)null);
    }

    [Test]
    public async Task SelectBooksAsync_ReturnBooksWithLimit_ExistingBooks()
    {
        var prefix = Guid.NewGuid().ToString();
        var limit = 10;
        var total = 20;
        var expectedBooks = new List<Book>();
        for (var i = 0; i < total; i++)
        {
            var expectedBook = new BookBuilder().WithName(prefix + Guid.NewGuid()).Build();
            expectedBooks.Add(expectedBook);
            var book = await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
            await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
                $"book {expectedBook.Id}", CancellationToken.None);
            bookIds.Add(expectedBook.Id.Value);
        }
    
        var actualBooks = await bookRepository.SelectBooksAsync(new BookFilter(){
                Query = prefix,
                Limit = limit
            }, CancellationToken.None);
    
        actualBooks.Should().BeEquivalentTo(expectedBooks.Take(limit));
    }
    
    [Test]
    public async Task GetBookSummaryBySynonymAsync_ReturnBookSummary_ExistingSynonyms()
    {
        var name = Guid.NewGuid().ToString();
        var book = new BookBuilder().WithName(name).Build();
        var resultBook = await bookRepository.SaveBookAsync(book, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(book.Id!.Value, book.GetTextForFts(),
            name, CancellationToken.None);
        bookIds.Add(book.Id.Value);

        var bookSummary = await bookRepository.GetBookSummaryBySynonymAsync(name, CancellationToken.None);
        var bookSummaries = await bookRepository.SelectBooksSummaryAsync(new BookFilter(){Query = name}, CancellationToken.None);

        bookSummary.Should().BeEquivalentTo(bookSummaries.FirstOrDefault());
    }
    
    [Test]
    public async Task GetBookSummaryBySynonymAsync_ReturnBookSummary_ExistingBook()
    {
        var name = Guid.NewGuid().ToString();
        var expectedBook = new BookBuilder().WithName(name).Build();
        var book = await bookRepository.SaveBookAsync(expectedBook, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(expectedBook.Id!.Value, expectedBook.GetTextForFts(),
            name, CancellationToken.None);
        bookIds.Add(expectedBook.Id.Value);
        
        var bookSummary = await bookRepository.GetBookSummaryBySynonymAsync(name, CancellationToken.None);
        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedBook.Name, bookSummary.Name);
            Assert.AreEqual(expectedBook.Author, bookSummary.Author);
            Assert.AreEqual(expectedBook.Description, bookSummary.Description);
            Assert.AreEqual(expectedBook.RubricId, bookSummary.RubricId);
            Assert.AreEqual(expectedBook.Id, bookSummary.Id);
            Assert.AreEqual(expectedBook.Name, bookSummary.Synonym);
        });
    }
    
    [Test]
    public async Task GetBookSummaryBySynonymAsync_ReturnBookSummary_BusyBook()
    {
        var name = Guid.NewGuid().ToString();
        var book = new BookBuilder().WithName(name).Build();
        var reader = new ReaderBuilder().WithBookId((int)book.Id).Build();
        var resultBook = await bookRepository.SaveBookAsync(book, CancellationToken.None);
        await bookRepository.SaveBookIndexAsync(book.Id!.Value, book.GetTextForFts(),
            name, CancellationToken.None);
        bookIds.Add(book.Id.Value);
        await bookRepository.SaveReaderAsync(reader, CancellationToken.None);

        var bookSummary = await bookRepository.GetBookSummaryBySynonymAsync(name, CancellationToken.None);
        bookSummary.IsBusy.Should().Be(true);
    }
    
    [Test]
    public async Task SelectBooksAsync_ReturnOrderedBooksWithOffset_ExistingBooks()
    {
        var prefix = Guid.NewGuid().ToString();
        var offset = 5;
        var total = 10;
        var expectedBooks = new List<Book>();
        for (var i = 0; i < total; i++)
        {
            await Task.Run(() =>
            {
                var book = new BookBuilder().WithName(prefix + Guid.NewGuid()).WithId(i).Build();
                var resultBook = bookRepository.SaveBookAsync(book, CancellationToken.None);
                bookRepository.SaveBookIndexAsync(book.Id!.Value, book.GetTextForFts(),
                    $"book {book.Id}", CancellationToken.None);
                bookIds.Add(book.Id.Value);
                expectedBooks.Add(book);
            });
        }

        var actualBooks = await bookRepository.SelectBooksAsync(new BookFilter(){
            Query = prefix,
            Offset = offset,
            Order = BookOrder.ByLastAdding,
        }, CancellationToken.None);

        expectedBooks.Reverse();
        actualBooks.Should().BeEquivalentTo(expectedBooks.Skip(offset));
    }
}