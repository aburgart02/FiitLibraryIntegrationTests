using System.Runtime.Serialization;
using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class Librarian: Entity
    {
        public string Name { get; set; }

        public Contact[] Contacts { get; set; }
    }
}