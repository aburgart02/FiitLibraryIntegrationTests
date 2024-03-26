using System.Text.RegularExpressions;

namespace Kontur.BigLibrary.Service.Validations
{
    public static class ValidationHelper
    {
        public static bool IsEnglishAbc(string str)
        {
            return Regex.Matches(str,@"[a-zA-Z_0-9]").Count == str.Length;
        }

    }
}