using System.Runtime.Serialization;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class BookSummary
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int RubricId { get; set; }

        [DataMember]
        public string RubricName { get; set; }

        [DataMember]
        public string RubricSynonym { get; set; }

        [DataMember]
        public int ImageId { get; set; }

        [DataMember]
        public string Synonym { get; set; }

        [DataMember]
        public bool IsBusy { get; set; }

        [DataMember]
        public string Price { get; set; }
    }
}