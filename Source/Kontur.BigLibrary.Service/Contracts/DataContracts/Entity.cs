using System.Runtime.Serialization;

namespace Kontur.BigLibrary.Service.Contracts.DataContracts
{
    [DataContract]
    public abstract class Entity
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public bool  IsDeleted { get; set; }
    }
}