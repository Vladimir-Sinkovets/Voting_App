using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;
using Voting_App.Services.Exceptions;
using Voting_App.Services.PasswordHash;

namespace Voting_App.Services.Authentication
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher _passwordHasher;

        public CookieAuthenticationService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
                throw new UserAlreadyRegisteredException($"User with email = {email} is already registered");

            var passwordHash = _passwordHasher.GetPasswordHash(password);

            _dbContext.Users.Add(new User { Email = email, PasswordHash = passwordHash});

            await _dbContext.SaveChangesAsync();

            await SignIn(email);
        }

        public async Task Login(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                throw new NotFoundException($"User with email = {email} does not exist");

            var isPasswordCorrect = _passwordHasher.VerifyPasswordHash(password, user.PasswordHash!);

            if (isPasswordCorrect == false)
                throw new IncorrectPasswordException($"Password is incorrect");

            await SignIn(email);
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task SignIn(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
