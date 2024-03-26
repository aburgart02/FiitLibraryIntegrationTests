using System;
using System.Collections.Generic;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.ImageService;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit.BookServiceTests;

public class BookServiceTestBase
{
    protected IBookService BookService;
    protected IImageService ImageService;
    protected IBookRepository BookRepository;
    protected IEventService EventService;
    protected ISynonymMaker SynonymMaker;

    [SetUp]
    public void Setup()
    {
        BookRepository = Substitute.For<IBookRepository>();
        ImageService = Substitute.For<IImageService>();
        EventService = Substitute.For<IEventService>();
        SynonymMaker = Substitute.For<ISynonymMaker>();
        BookService = new BookService(BookRepository, ImageService, EventService, SynonymMaker);
    }

    protected static Book GenerateBook(int? id)
    {
        return new Book
        {
            Id = id,
            Name = "bookName",
            Author = "bookAuthor",
            Description = "bookDescription",
            RubricId = 1,
            ImageId = 1
        };
    }

    protected static BookSummary GenerateBookSummary(int id, string synonym = "")
    {
        return new BookSummary()
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Author = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            RubricId = 1,
            ImageId = 1,
            IsDeleted = false,
            RubricName = Guid.NewGuid().ToString(),
            RubricSynonym = Guid.NewGuid().ToString(),
            IsBusy = false,
            Synonym = synonym
        };
    }
    protected static IReadOnlyList<BookSummary> GenerateBooksSummary(int id, string synonym = "")
    {
        List<BookSummary> bookSummary = new List<BookSummary>()
        {
            new BookSummary()
            {
                Id = id,
                Name = "bookName",
                Author = "bookAuthor",
                Description = "bookDescription",
                RubricId = 1,
                ImageId = 1,
                IsDeleted = false,
                RubricName = Guid.NewGuid().ToString(),
                RubricSynonym = Guid.NewGuid().ToString(),
                IsBusy = false,
                Synonym = synonym
            }
        };
        return bookSummary;
    }
    
    protected static IReadOnlyList<BookSummary> GenerateBusyBooksSummary(int id, string synonym = "")
    {
        List<BookSummary> bookSummary = new List<BookSummary>()
        {
            new BookSummary()
            {
                Id = id,
                Name = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                RubricId = 1,
                ImageId = 1,
                IsDeleted = false,
                RubricName = Guid.NewGuid().ToString(),
                RubricSynonym = Guid.NewGuid().ToString(),
                IsBusy = true,
                Synonym = synonym
            }
        };
        
        return bookSummary;
    }
    protected static Reader GenerateReader(int? id, int bookId, bool isDeleted = false)
    {
        return new Reader
        {
            Id = id,
            UserName = Guid.NewGuid().ToString(),
            StartDate = DateTime.Now,
            BookId = bookId,
            IsDeleted = isDeleted
        };
    }
    protected static IReadOnlyList<Reader> GenerateReaders(int? id, int bookId, string? userName, bool isDeleted = false)
    {
        List<Reader> reader = new List<Reader>()
        {
            new Reader()
            {
                Id = id,
                UserName = userName,
                StartDate = DateTime.Now,
                BookId = bookId,
                IsDeleted = isDeleted
            }
        };
        
        return reader;
    }
    protected static ReaderInQueue GenerateReaderInQueue(int? id, int bookId, bool isDeleted = false)
    {
        return new ReaderInQueue
        {
            Id = id,
            UserName = Guid.NewGuid().ToString(),
            StartDate = DateTime.Now,
            BookId = bookId,
            IsDeleted = isDeleted
        };
    }
}