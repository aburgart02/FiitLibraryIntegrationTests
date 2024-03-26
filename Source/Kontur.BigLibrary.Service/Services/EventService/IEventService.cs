using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Events;

namespace Kontur.BigLibrary.Service.Services.EventService
{
    public interface IEventService
    {
        public Task PublishEventAsync(ChangedEvent @event, CancellationToken cancellationToken);
    }
}