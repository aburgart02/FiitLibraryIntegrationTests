using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Tests.Core.Helpers;

public class BookTestModel
{
    public string Name { get; set; } = $"Default name {IntGenerator.Get()}";

    public string Author { get; set; } = "Default author";

    public string Description { get; set; } = "Default description";

    public int RubricId { get; set; } = 1;

    public int ImageId { get; set; } = 1;

    public int Count { get; set; } = 1;

    public string Price { get; set; } = "";

    public int Id { get; set; } = IntGenerator.Get();

    public Book ToBook()
        => new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Author = Author,
            RubricId = RubricId,
            Count = Count,
            Price = Price,
            ImageId = ImageId
        };
}