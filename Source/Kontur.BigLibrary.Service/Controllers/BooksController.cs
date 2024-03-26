using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Filters;
using Kontur.BigLibrary.Service.Services.BookService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kontur.BigLibrary.Service.Controllers
{
    [Authorize]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService) =>
            this.bookService = bookService;

        [HttpGet("{id}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<Book>> Get(int id) =>
            await bookService.GetBookAsync(id, CancellationToken.None);

        [HttpGet("synonym/{synonym}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<Book>> Get(string synonym) =>
            await bookService.GetBookBySynonymAsync(synonym, CancellationToken.None);

        [HttpGet("summary/{synonym}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<BookSummary>> GetSummary(string synonym) =>
            await bookService.GetBookSummaryBySynonymAsync(synonym, CancellationToken.None);

        [HttpGet("summary/select")]
        public async Task<ActionResult<IEnumerable<Book>>> SelectSummary([FromQuery] BookFilter parameters)
        {
            var filter = new BookFilter()
            {
                Query = parameters.Query,
                RubricSynonym = parameters.RubricSynonym,
                Order = BuildBookOrder(parameters.Query),
                IsBusy = parameters.IsBusy,
                Limit = parameters.Limit,
                Offset = parameters.Offset
            };

            var result = await bookService.SelectBooksSummaryAsync(filter, CancellationToken.None);

            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportBooksToXml([FromQuery] BookFilter parameters)
        {
            var filter = new BookFilter()
            {
                Query = parameters.Query,
                RubricSynonym = parameters.RubricSynonym,
                Order = BuildBookOrder(parameters.Query),
                IsBusy = parameters.IsBusy,
                Limit = parameters.Limit,
                Offset = parameters.Offset
            };

            var xmlString = await bookService.ExportBooksToXmlAsync(filter, CancellationToken.None);

            // Отправляем XML-строку как ответ
            return Content(xmlString, "application/xml");
        }

        [HttpPost]
        public async Task<ActionResult<Book>> SaveBook([FromBody] Book book)
        {
            try
            {
                var result = await bookService.SaveBookAsync(book, CancellationToken.None);
                return Ok(result);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
            

        [HttpPost("readers")]
        public async Task<ActionResult<IEnumerable<Reader>>> SaveReaders(int bookId, [FromBody] Reader[] readers)
        {
            var result = await bookService.SaveReadersAsync(bookId, readers, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("readers/{bookId}")]
        public async Task<ActionResult<IEnumerable<Reader>>> SelectReaders(int bookId)
        {
            var result = await bookService.SelectReadersAsync(bookId, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("readersInQueue/{bookId}")]
        public async Task<ActionResult<IEnumerable<Reader>>> SelectReadersInQueue(int bookId)
        {
            var result = await bookService.SelectReadersInQueueAsync(bookId, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> CheckoutBook(int bookId, string userName)
        {
            var result = await bookService.CheckoutBookAsync(bookId, userName);
            return Ok(new { message = result });
        }

        [HttpGet("enqueue")]
        public async Task<IActionResult> Enqueue(int bookId, string userName)
        {
            var result = await bookService.EnqueueAsync(bookId, userName);
            return Ok(new { message = result });
        }

        [HttpGet("return")]
        public async Task<IActionResult> ReturnBook(int bookId, string userName)
        {
            var result = await bookService.ReturnBookAsync(bookId, userName);
            return Ok(new { message = result });
        }

        private BookOrder BuildBookOrder(string query) =>
            string.IsNullOrEmpty(query) ? BookOrder.ByLastAdding : BookOrder.ByRankAndLastAdding;
    }
}
