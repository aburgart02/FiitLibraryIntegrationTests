using System;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.EventService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Kontur.BigLibrary.Service.Services.SynonimMaker;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Kontur.BigLibrary.Tests.Integration.BookServiceTests;

public class ContainerWithMocks
{
    public IServiceCollection _collection;

    public class MockDataBase : IDbConnectionFactory
    {
        public async Task<IDbConnection> OpenAsync(CancellationToken cancellation)
        {
            return new SQLiteConnection();
        }
    }
    
    public ContainerWithMocks()
    {
        _collection = new ServiceCollection();
        
        _collection.AddSingleton<IDbConnectionFactory, MockDataBase>();

        _collection.AddSingleton<ISynonymMaker, SynonymMaker>();

        _collection.AddSingleton(_ => Substitute.For<IBookRepository>());
        _collection.AddSingleton<IBookService, BookService>();

        _collection.AddSingleton<IImageService, ImageService>();
        _collection.AddSingleton<IImageTransformer, ImageTransformer>();
        _collection.AddSingleton(_ => Substitute.For<IImageRepository>());

        _collection.AddSingleton(_ => Substitute.For<IEventRepository>());
        _collection.AddSingleton<IEventService, EventService>();
    }

    public IServiceProvider Build()
    {
        return _collection.BuildServiceProvider();
    }
}