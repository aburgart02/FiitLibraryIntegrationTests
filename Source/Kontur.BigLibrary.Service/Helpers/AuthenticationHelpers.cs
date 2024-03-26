using System.Security.Claims;
using Kontur.BigLibrary.Service.Contracts;
using Microsoft.AspNetCore.Http;

namespace Kontur.BigLibrary.Service.Helpers
{
    public class AuthenticationHelpers : IAuthenticationHelpers
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationHelpers(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        private string FindClaim(string type) => httpContextAccessor.HttpContext.User.FindFirst(type)?.Value;

        public User GetUserInfo() =>
            new User
            {
                Login = FindClaim(ClaimTypes.Email),
                Name = FindClaim(ClaimTypes.Name),
            };
    }
}