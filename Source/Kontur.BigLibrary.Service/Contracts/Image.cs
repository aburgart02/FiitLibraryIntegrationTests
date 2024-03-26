using Kontur.BigLibrary.Service.Contracts.DataContracts;

namespace Kontur.BigLibrary.Service.Contracts
{
    public class Image: Entity
    {
        public byte[] Data { get; set; }
    }
}