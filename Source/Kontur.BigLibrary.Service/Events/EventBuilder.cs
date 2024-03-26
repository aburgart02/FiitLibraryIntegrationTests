using System;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Events
{
    public static class EventBuilder
    {
        private static ChangedEvent CreateChangedEvent<T>(T entity, string sourceType) where T : Entity
        {
            return new ChangedEvent()
            {
                SourceId = entity.Id.Value,
                Timestamp = DateTime.UtcNow, 
                Source = entity,
                SourceType = sourceType
            };
        }

        public static ChangedEvent CreateChangedEvent(this Book book) => CreateChangedEvent(book, nameof(Book));
        public static ChangedEvent CreateChangedEvent(this Rubric rubric) => CreateChangedEvent(rubric, nameof(Rubric));
        public static ChangedEvent CreateChangedEvent(this Librarian librarian) => CreateChangedEvent(librarian, nameof(Librarian));
        public static ChangedEvent CreateChangedEvent(this Reader reader) => CreateChangedEvent(reader, nameof(Reader));
        public static ChangedEvent CreateChangedEvent(this ReaderInQueue reader) => CreateChangedEvent(reader, nameof(ReaderInQueue));
    }
}