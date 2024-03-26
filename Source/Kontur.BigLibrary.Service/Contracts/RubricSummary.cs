using System.Runtime.Serialization;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class RubricSummary
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public string Synonym { get; set; }
    }
}