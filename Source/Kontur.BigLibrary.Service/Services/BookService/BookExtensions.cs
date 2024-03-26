using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.BookService
{
    public static class BookExtensions
    {
        public static string GetTextForFts(this Book book) => $"{book.Name} {book.Author} {book.Description}";
    }
}