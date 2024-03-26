using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Models;

namespace Kontur.BigLibrary.Service.Services.AuthService
{
    public interface IAuthService
    {
        Task<(string token, string messageType, string message)> LoginAsync(UserLoginModel model, bool modelStateIsValid);
        Task<(string token, string messageType, string message)> RegisterAsync(UserLoginModel model, bool modelStateIsValid);
        Task<bool> ValidatePasswordAsync(string password);
    }
}