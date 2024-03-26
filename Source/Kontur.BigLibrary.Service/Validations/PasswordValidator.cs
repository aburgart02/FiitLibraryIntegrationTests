namespace Kontur.BigLibrary.Service.Validations
{
    public class PasswordValidator
    {
        public static (bool IsValid, string Strength) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "Invalid");
            }

            if (password.Length < 6 || password.Length > 15)
            {
                return (false, "Weak");
            }

            bool hasDigit = false;
            bool hasLowerCase = false;
            bool hasUpperCase = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsLetter(c))
                {
                    char lower = char.ToLower(c);
                    char upper = char.ToUpper(c);

                    if (c == lower)
                    {
                        hasLowerCase = true;
                    }
                    else if (c == upper)
                    {
                        hasUpperCase = true;
                    }
                }
                else if (char.IsSymbol(c) || char.IsPunctuation(c))
                {
                    hasSpecialChar = true;
                }
            }

            if (hasDigit && hasLowerCase && hasUpperCase && hasSpecialChar)
            {
                if (password.Length <= 10)
                {
                    return (true, "Good");
                }
                else if (password.Length >= 11 && password.Length <= 15)
                {
                    return (true, "Strong");
                }
            }

            return (false, "Weak");
        }
    }
}