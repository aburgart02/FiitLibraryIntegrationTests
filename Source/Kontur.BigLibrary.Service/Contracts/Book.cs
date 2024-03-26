using System.Runtime.Serialization;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class Book : Entity
    {
        [DataMember] public string Name { get; set; }

        [DataMember] public string Author { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public int RubricId { get; set; }

        [DataMember] public int Count = 1;

        [DataMember] public int ImageId { get; set; }


        [DataMember] public string Price { get; set; }
    }
}