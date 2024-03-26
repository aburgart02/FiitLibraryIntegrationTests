namespace Kontur.BigLibrary.Tests.Core.Helpers.StringGenerator;

public static class StringGenerator
{
    private static Random random = new Random();


    public static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789 ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GetEmail()
    {
        return $"{Guid.NewGuid()}@test.com";
    }
    
    public static string GetValidPassword()
    {
        return $"Test@123456";
    }
}