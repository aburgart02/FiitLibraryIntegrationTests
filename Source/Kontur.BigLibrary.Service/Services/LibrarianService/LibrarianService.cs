using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Exceptions;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;

namespace Kontur.BigLibrary.Service.Services.LibrarianService;

public class LibrarianService : ILibrarianService
{
    private readonly IBookRepository bookRepository;
    private readonly IEventService eventService;
    public LibrarianService(IBookRepository bookRepository, IEventService eventService)
    {
        this.bookRepository = bookRepository;
        this.eventService = eventService;
    }

    public async Task<Librarian> GetLibrarianAsync(int id, CancellationToken cancellation) =>
        await bookRepository.GetLibrarianAsync(id, cancellation);

    public async Task<IReadOnlyList<Librarian>> SelectLibrariansAsync(CancellationToken cancellation) =>
        await bookRepository.SelectLibrariansAsync(cancellation);

    public async Task<Librarian> SaveLibrarianAsync(Librarian librarian, CancellationToken cancellation)
    {
        librarian.Id ??= await bookRepository.GetNextLibrarianIdAsync(cancellation);
        var actualLibrarian = await bookRepository.SaveLibrarianAsync(librarian, cancellation);

        var @event =librarian.CreateChangedEvent();
        await eventService.PublishEventAsync(@event, cancellation);

        return actualLibrarian;
    }

    public async Task DeleteLibrarianAsync(int id, CancellationToken cancellation)
    {
        var librarian = await bookRepository.GetLibrarianAsync(id, cancellation);
        if (librarian == null)
        {
            throw new EntityNotFoundException();
        }

        librarian.IsDeleted = true;

        var actualLibrarian = await bookRepository.SaveLibrarianAsync(librarian, cancellation);
        
        var @event =librarian.CreateChangedEvent();
        await eventService.PublishEventAsync(@event, cancellation);
    }
}