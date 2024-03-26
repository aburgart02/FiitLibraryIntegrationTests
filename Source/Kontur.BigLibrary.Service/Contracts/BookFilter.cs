using System.Runtime.Serialization;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class BookFilter
    {
        [DataMember]
        public string Query { get; set; }

        [DataMember]
        public BookOrder Order { get; set; }

        [DataMember]
        public bool? IsBusy { get; set; }

        [DataMember]
        public string RubricSynonym { get; set; }
        
        [DataMember]
        public string Synonym { get; set; }

        [DataMember]
        public int? Limit { get; set; }

        [DataMember]
        public int? Offset { get; set; }
    }
}