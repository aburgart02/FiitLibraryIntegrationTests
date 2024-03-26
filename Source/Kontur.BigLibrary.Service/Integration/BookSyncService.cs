using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Models;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.RubricsService;
using Vostok.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Kontur.BigLibrary.Service.Integration
{
    public class BookInsertionOptions
    {
        public SyncOptionsEnum SyncOptions { get; set; }
        public BookData ExternalBook { get; set; }
        public Dictionary<string, RubricSummary> RubricsByName { get; set; }
        public Dictionary<int, string> BooksDescription { get; set; }
        public string Price { get; set; }
        public CancellationToken StoppingToken { get; set; }
    }

    public class BookSyncService : BackgroundService
    {
        private readonly ILog logger;
        private readonly IBookService bookService;
        private readonly IImageService imageService;
        private readonly IRubricsService rubricsService;

        private readonly IConfiguration configuration;


        public BookSyncService(ILog logger, IBookService bookService, IImageService imageService,
            IRubricsService rubricsService, IConfiguration configuration)
        {
            this.logger = logger;
            this.bookService = bookService;
            this.imageService = imageService;
            this.rubricsService = rubricsService;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.Info("Начинаем синхронизировать книги");

            try
            {
                await SyncBooksAsync(SyncOptionsEnum.InsertAndUpdate, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при синхронизации книг");
            }

            logger.Info("Синхронизация книг закончена");
        }

        private async Task SyncBooksAsync(SyncOptionsEnum syncOptionsEnum, CancellationToken stoppingToken)
        {
            var booksFileContent = await File.ReadAllTextAsync("Data\\Books.json", stoppingToken);
            var descriptionsFileContent = await File.ReadAllTextAsync("Data\\Descriptions.json", stoppingToken);
            var externalBooks = JsonConvert.DeserializeObject<BookData[]>(booksFileContent);
            var booksDescription = JsonConvert.DeserializeObject<Dictionary<int, string>>(descriptionsFileContent);
            var rubricsByName = await GetRubricsByName(stoppingToken);

            logger.Info(string.Join("; ","\n\nРубрики", rubricsByName.Select(x=> x.Key+" Id:"+ x.Value.Id)));
            var googleSheetsIntegration = new GoogleSheetsIntegration();
            string spreadsheetId = configuration["GoogleDriveIntegration:spreadsheetId"];
            
            logger.Info($"_____________________________________________\n\nАйди таблицы {spreadsheetId}\n\n_______________________________________");
            
            string range = configuration["GoogleDriveIntegration:range"];

            var data = googleSheetsIntegration.ReadData(spreadsheetId, range);
            Dictionary<int, string> dataDictionary = new Dictionary<int, string>();

            bool isFirstRow = true;

            if (data != null && data.Count > 0)
            {
               
                logger.Info("Конвертируем данные из таблицы.");
                foreach (var row in data)
                {
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    if (row.Count >= 4)
                    {
                        int id = -1;
                        string value = row[3].ToString();

                        if (int.TryParse(row[1].ToString(), out id))
                        {
                            //     logger.Info("Получили индекс {id}", id);
                        }
                        else
                        {
                            logger.Info("В таблице не оказалось идентификатора");
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            //    logger.Info("Получили значение {value}", value);
                        }
                        else
                        {
                            logger.Info("В таблице не оказалось ценника");
                            logger.Info(value);
                            continue;
                        }

                        dataDictionary[id] = value;
                    }
                }
            }

            foreach (var externalBook in externalBooks)
            {
                try
                {
                    string price = "0";

                    if (dataDictionary.TryGetValue(externalBook.Id, out price))
                    {
                        logger.Info("Удалось сопоставить книгу с записью в таблице. Id: {Id}", externalBook.Id);
                    }

                    var book = await bookService.GetBookAsync(externalBook.Id, stoppingToken);
                    if (book != null)
                    {
                        await CheckAndUpdateBookAsync(syncOptionsEnum, book, externalBook, booksDescription, price,
                            stoppingToken);
                        continue;
                    }

                   
                    await InsertBookAsync(new BookInsertionOptions()
                    {
                        BooksDescription = booksDescription,
                        ExternalBook = externalBook,
                        Price = price,
                        RubricsByName = rubricsByName,
                        StoppingToken = stoppingToken,
                        SyncOptions = syncOptionsEnum
                    });
                }
                catch (Exception ex)
                {
                    logger.Debug(ex, "Ошибка синхронизации книги с идентификатором {id}", externalBook.Id);
                }
            }
        }

        private async Task<Dictionary<string, RubricSummary>> GetRubricsByName(CancellationToken stoppingToken)
        {
            var groupsRubrics = await rubricsService.SelectGroupsRubricSummaryAsync(stoppingToken);
            return groupsRubrics.SelectMany(x => x.Rubrics).ToDictionary(x => x.Name);
        }

        private async Task InsertBookAsync(BookInsertionOptions options)
        {
            if (options.SyncOptions == SyncOptionsEnum.OnlyUpdate)
            {
                return;
            }

            var imageBytes = await ImageBytesProvider.ProvideAsync(options.ExternalBook.ImageId, options.StoppingToken);
            if (imageBytes == null)
            {
                return;
            }

            await imageService.SaveAsync(new Image() { Id = options.ExternalBook.ImageId, Data = imageBytes },
                options.StoppingToken);

            var rubricId = Constants.DefaultRubricId;
            logger.Info($"Рубрика книги:{options.ExternalBook.RubricName}\n ");
            if (options.RubricsByName.ContainsKey(options.ExternalBook.RubricName))
            {
                logger.Info($"Рубрика есть в списке");
                rubricId = options.RubricsByName[options.ExternalBook.RubricName].Id;
            }
            else
            {
                logger.Info($"Рубрики нет в списке ");
            }

            if (rubricId == Constants.DefaultRubricId)
            {
                rubricId = Constants.OtherRubricId;
            }

            var book = new Book()
            {
                Id = options.ExternalBook.Id,
                Author = options.ExternalBook.Author,
                Name = options.ExternalBook.Name,
                ImageId = options.ExternalBook.ImageId,
                RubricId = rubricId,
                Description = options.BooksDescription.TryGetValue(options.ExternalBook.Id, out var description)
                    ? description
                    : null,
                Price = options.Price
            };

            try
            {
                await bookService.SaveBookAsync(book, options.StoppingToken);

                if (options.ExternalBook.Readers.Any())
                {
                    var readers = options.ExternalBook.Readers.Select(x => x.ToReader(options.ExternalBook.Id))
                        .ToArray();
                    await bookService.SaveReadersAsync(book.Id.Value, readers, options.StoppingToken);
                }


                logger.Debug("Добавлена новая книга: {Name}. Id: {Id}", book.Name, book.Id);
            }
            catch (ValidationException vex)
            {
                logger.Debug("Не импортировали: {Name}. Error: {Message}", book.Name, vex.Message);
            }
        }

        private async Task CheckAndUpdateBookReadersAsync(BookData externalBook, CancellationToken stoppingToken)
        {
            var result = false;

            var readers = await bookService.SelectReadersAsync(externalBook.Id, stoppingToken);
            var readerActions = readers.Select(x => x.ToBookAction()).ToArray();

            if (IsNotEquals(readerActions, externalBook.Readers))
            {
                var newReaders = externalBook.Readers.Select(x => x.ToReader(externalBook.Id)).ToArray();
                await bookService.SaveReadersAsync(externalBook.Id, newReaders, stoppingToken);
                result = true;
            }

            var readersInQueue = await bookService.SelectReadersInQueueAsync(externalBook.Id, stoppingToken);
            var readerInQueueActions = readersInQueue.Select(x => x.ToBookAction()).ToArray();

            if (IsNotEquals(readerInQueueActions, externalBook.ReadersInQueue))
            {
                var newReaders = externalBook.ReadersInQueue.Select(x => x.ToReaderInQueue(externalBook.Id)).ToArray();
                await bookService.SaveReadersInQueueAsync(externalBook.Id, newReaders, stoppingToken);
                result = true;
            }

            if (result)
            {
                logger.Debug("Обновлены читатели для книги: {Name}. Id: {Id}", externalBook.Name, externalBook.Id);
            }
        }

        private async Task CheckAndUpdateBookAsync(SyncOptionsEnum syncOptionsEnum, Book book, BookData externalBook,
            Dictionary<int, string> booksDescription, string price, CancellationToken stoppingToken)
        {
            if (syncOptionsEnum == SyncOptionsEnum.OnlyInsert)
            {
                return;
            }

            var changed = false;
            var newDescription =
                booksDescription.TryGetValue(externalBook.Id, out var description) ? description : null;

            if (book.Name != externalBook.Name)
            {
                book.Name = externalBook.Name;
                changed = true;
            }

            if (book.Author != externalBook.Author)
            {
                book.Author = externalBook.Author;
                changed = true;
            }
            

            if (newDescription != null && book.Description != newDescription)
            {
                book.Description = newDescription;
                changed = true;
            }

            if (book.Price != price)
            {
                book.Price = price;
                changed = true;
            }

            if (changed)
            {
                await bookService.SaveBookAsync(book, stoppingToken);
                logger.Debug("Обновлена книга: {Name}. Id:{Id}", externalBook.Name, externalBook.Id);
            }

            await CheckAndUpdateBookReadersAsync(externalBook, stoppingToken);
        }

        private bool IsNotEquals(BookAction[] savedBookActions, BookAction[] externalBookReaders)
        {
            return !savedBookActions.OrderBy(x => x.UserName).ThenBy(x => x.StartDate)
                .SequenceEqual(externalBookReaders.OrderBy(x => x.UserName).ThenBy(x => x.StartDate));
        }
    }
}