using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Events;

namespace Kontur.BigLibrary.Service.Services.EventService.Repository
{
    public interface IEventRepository
    {
        public Task SaveAsync(ChangedEvent @event, CancellationToken cancellationToken);
    }
}