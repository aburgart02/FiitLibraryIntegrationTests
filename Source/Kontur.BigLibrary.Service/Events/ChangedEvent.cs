using System;
using System.Runtime.Serialization;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Events
{
    [DataContract]
    public class ChangedEvent
    {
        [DataMember]
        public long? SequenceNumber { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public string SourceType { get; set; }

        [DataMember]
        public int SourceId { get; set; }

        [DataMember]
        public Entity Source { get; set; }
    }
}