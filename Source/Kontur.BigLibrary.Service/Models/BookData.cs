using System;

namespace Kontur.BigLibrary.Service.Models
{
    public class BookData
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string RubricName { get; set; }
        public int ImageId { get; set; }
        public int Count { get; set; }
        public BookAction[] Readers { get; set; } = Array.Empty<BookAction>();
        public BookAction[] ReadersInQueue { get; set; } = Array.Empty<BookAction>();
    }
}