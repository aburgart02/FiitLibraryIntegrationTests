using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Tests.Core.Helpers;

public class ReaderBuilder
{
    private int bookId;
    private string userName = "";
    private DateTime startDate;

    public ReaderBuilder WithBookId(int bookId)
    {
        this.bookId = bookId;
        return this;
    }

    public ReaderBuilder WithUserName(string userName)
    {
        this.userName = userName;
        return this;
    }

    public ReaderBuilder WithStartDate(DateTime startDate)
    {
        this.startDate = startDate;
        return this;
    }

    public Reader Build() => new()
    {
        BookId = bookId,
        UserName = userName,
        StartDate = startDate,
    };
}