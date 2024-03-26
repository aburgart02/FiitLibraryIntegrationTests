using System;

namespace Kontur.BigLibrary.Service.Exceptions
 {
     public class ValidationException : Exception
     {
         public ValidationException(string message)
             : base(message)
         {
         }
     }
 }