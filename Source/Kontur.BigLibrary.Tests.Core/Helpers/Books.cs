using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Tests.Core.Helpers;

public static class Books
{
    public static readonly Book EnglishBook = new BookBuilder()
        .WithId(1)
        .WithName("Database Systems. The Complete Book")
        .WithAuthor("Hector Garcia-Molina, Jeffrey D.Ullman, Jennifer Widom")
        .Build();

    public static readonly Book RussianBook =
        new BookBuilder()
            .WithId(2)
            .WithName("Вишневый сад").WithAuthor("Чехов")
            .Build();
}