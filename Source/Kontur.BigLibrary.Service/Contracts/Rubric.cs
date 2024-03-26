using System.Runtime.Serialization;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class Rubric : Entity
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public int? OrderId { get; set; }
    }
}