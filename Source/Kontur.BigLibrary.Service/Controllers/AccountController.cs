using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Models;
using Kontur.BigLibrary.Service.Services.AuthService;

namespace Kontur.BigLibrary.Service.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService authService;

        public AccountController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel model)
        {
            var (token, messageType, message) = await authService.LoginAsync(model, ModelState.IsValid);
            if (token == null)
            {
                ModelState.AddModelError(messageType, message);
                return BadRequest(ModelState);
            }

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserLoginModel model)
        {
            var (token, messageType, message) = await authService.RegisterAsync(model, ModelState.IsValid);
            if (token == null)
            {
                ModelState.AddModelError(messageType, message);
                return BadRequest(ModelState);
            }

            return Ok(new { token });
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidatePasswordAsync(string password)
        {
            var isValid = await authService.ValidatePasswordAsync(password);

            if (isValid)
            {
                return Ok(new { IsValid = true });
            }
            else
            {
                ModelState.AddModelError("Password", "Невалидный пароль.");
                return BadRequest(ModelState);
            }
        }
    }

}