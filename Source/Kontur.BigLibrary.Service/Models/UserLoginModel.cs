using System.ComponentModel.DataAnnotations;

namespace Kontur.BigLibrary.Service.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите email.")]
        [EmailAddress(ErrorMessage = "Неправильный формат email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}