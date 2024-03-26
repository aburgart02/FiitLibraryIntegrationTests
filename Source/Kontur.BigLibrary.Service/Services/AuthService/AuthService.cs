using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Models;
using Kontur.BigLibrary.Service.Validations;

namespace Kontur.BigLibrary.Service.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public AuthService(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<(string token, string messageType, string message)> LoginAsync(UserLoginModel model, bool modelStateIsValid)
        {
            if (!modelStateIsValid)
            {
                return (null, "Email", "Не удалось авторизоваться.");
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return (null, "Email", "Такой пользователь не существует.");
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return (GenerateJwtToken(user), null, null);
            }
            else
            {
                return (null, "Email", "Неверный пользователь или пароль.");
            }
        }

        public async Task<(string token, string messageType, string message)> RegisterAsync(UserLoginModel model, bool modelStateIsValid)
        {
            if (!modelStateIsValid)
            {
                return (null, "Email", "Регистрация не удалась.");
            }

            var (isValidPassword, passwordStrength) = PasswordValidator.ValidatePassword(model.Password);

            if (!isValidPassword)
            {
                return (null, "Password", "Пароль не соответствует требованиям.");
            }

            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return (null, "Email", "Такой пользователь уже существует.");
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return (GenerateJwtToken(user),null,null);
            }
            else
            {
                return (null, "Email", "Регистрация не удалась.");
            }
        }

        public Task<bool> ValidatePasswordAsync(string password)
        {
            var (isValid, strength) = PasswordValidator.ValidatePassword(password);
            return Task.FromResult(isValid);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:DurationInHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = configuration["JwtSettings:Audience"],
                Issuer = configuration["JwtSettings:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}