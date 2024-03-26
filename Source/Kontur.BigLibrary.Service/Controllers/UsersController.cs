using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kontur.BigLibrary.Service.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationHelpers authenticationHelper;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsersController(
            IAuthenticationHelpers authenticationHelper,
            SignInManager<IdentityUser> signInManager)
        {
            this.authenticationHelper = authenticationHelper;
            this.signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var userInfo = authenticationHelper.GetUserInfo();
            return Ok(userInfo);
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await signInManager.SignOutAsync();
            return Ok(new { message = "User signed out successfully." });
        }
    }
}