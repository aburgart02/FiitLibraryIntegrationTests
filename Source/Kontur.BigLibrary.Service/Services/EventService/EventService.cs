using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Services.EventService.Repository;

namespace Kontur.BigLibrary.Service.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;

        public EventService(IEventRepository eventRepository) => this.eventRepository = eventRepository;

        public async Task PublishEventAsync(ChangedEvent @event, CancellationToken cancellationToken) =>
            await eventRepository.SaveAsync(@event, cancellationToken);
    }
}