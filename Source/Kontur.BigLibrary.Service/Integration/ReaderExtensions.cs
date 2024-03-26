using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Models;

namespace Kontur.BigLibrary.Service.Integration
{
    public static class ReaderExtensions
    {
        public static Reader ToReader(this BookAction bookAction, int bookId)
        {
            return new Reader()
            {
                BookId =  bookId,
                UserName = bookAction.UserName,
                StartDate = bookAction.StartDate
            };
        }

        public static ReaderInQueue ToReaderInQueue(this BookAction bookAction, int bookId)
        {
            return new ReaderInQueue()
            {
                BookId =  bookId,
                UserName = bookAction.UserName,
                StartDate = bookAction.StartDate
            };
        }

        public static BookAction ToBookAction(this Reader reader)
        {
            return new BookAction(reader.UserName, reader.StartDate);
        }

        public static BookAction ToBookAction(this ReaderInQueue reader)
        {
            return new BookAction(reader.UserName, reader.StartDate);
        }
    }
}