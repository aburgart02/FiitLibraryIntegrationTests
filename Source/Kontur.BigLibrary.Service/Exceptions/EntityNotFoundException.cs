using System;

namespace Kontur.BigLibrary.Service.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base("Сущность не найдена.")
        {
        }
    }
}