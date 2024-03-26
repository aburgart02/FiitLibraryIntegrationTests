using System;
using System.Runtime.Serialization;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class Reader:Entity
    {
        [DataMember]
        public int BookId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }
    }
}