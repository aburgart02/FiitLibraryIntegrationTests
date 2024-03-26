using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Filters;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.LibrarianService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kontur.BigLibrary.Service.Controllers
{
    [Authorize]
    [Route("api/librarians")]
    [ApiController]
    public class LibrariansController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly ILibrarianService librarianService;

        public LibrariansController(IBookService bookService, ILibrarianService librarianService)
        {
            this.bookService = bookService;
            this.librarianService = librarianService;
        }

        [HttpGet("{id}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<Librarian>> Get(int id) =>
            await librarianService.GetLibrarianAsync(id, CancellationToken.None);

        [HttpGet("select")]
        public async Task<ActionResult<IEnumerable<Librarian>>> Select()
        {
            var result = await librarianService.SelectLibrariansAsync(CancellationToken.None);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Librarian>> Save([FromBody] Librarian librarian) =>
            await librarianService.SaveLibrarianAsync(librarian, CancellationToken.None);

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await librarianService.DeleteLibrarianAsync(id, CancellationToken.None);
            return Ok();
        }
    }
}