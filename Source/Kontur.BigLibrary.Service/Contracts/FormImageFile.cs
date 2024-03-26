using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Kontur.BigLibrary.Service.Contracts
{
    [DataContract]
    public class FormImageFile
    {
        [DataMember]
        public IFormFile File { get; set; }
    }
}