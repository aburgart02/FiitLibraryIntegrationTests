using System;
using FluentValidation;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.EventService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Kontur.BigLibrary.Service.Services.RubricsService;
using Kontur.BigLibrary.Service.Services.SynonimMaker;
using Kontur.BigLibrary.Service.Validations;
using Kontur.BigLibrary.Tests.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Kontur.BigLibrary.Tests.Integration.BookServiceTests;

public class ContainerWithRealBd
{
    public IServiceCollection _collection;

    public ContainerWithRealBd()
    {
        _collection = new ServiceCollection();

        _collection.AddSingleton<IDbConnectionFactory>(x => new DbConnectionFactory(DbHelper.ConnectionString));

        _collection.AddSingleton<ISynonymMaker, SynonymMaker>();
        _collection.AddSingleton<IBookRepository, BookRepository>();
        _collection.AddSingleton<IValidator<Book>, BookValidator>();
        _collection.AddSingleton<IBookService, BookService>();

        _collection.AddSingleton<IImageService, ImageService>();
        _collection.AddSingleton<IImageTransformer, ImageTransformer>();
        _collection.AddSingleton<IImageRepository, ImageRepository>();
        _collection.AddSingleton<IValidator<FormImageFile>, ImageValidator>();

        _collection.AddSingleton<IEventRepository, EventRepository>();
        _collection.AddSingleton<IEventService, EventService>();

        _collection.AddSingleton<IValidator<Rubric>, RubricValidator>();
        _collection.AddSingleton<IRubricsService, RubricsService>();
    }

    public IServiceProvider Build()
    {
        return _collection.BuildServiceProvider();
    }
}