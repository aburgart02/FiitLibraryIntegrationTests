using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Events;

namespace Kontur.BigLibrary.Service.Services.EventService.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public EventRepository(IDbConnectionFactory connectionFactory) => this.connectionFactory = connectionFactory;

        public async Task SaveAsync(ChangedEvent @event, CancellationToken cancellationToken)
        {
            using var db = await connectionFactory.OpenAsync(cancellationToken);
            
            var parameters = new
            {
                @event.Timestamp,
                @event.SourceType,
                @event.SourceId,
                @event.Source,
            };

            await db.ExecuteAsync(saveChangedEventSql, parameters);
        }

        private static readonly string saveChangedEventSql = @"
        insert into changed_event(timestamp, 
                                  source_type, 
                                  source_id, 
                                  source)
        values(@Timestamp, @SourceType, @SourceId, @Source);";
    }
}