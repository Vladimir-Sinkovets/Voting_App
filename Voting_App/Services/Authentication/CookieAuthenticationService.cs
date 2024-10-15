using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;
using Voting_App.Services.Exceptions;

namespace Voting_App.Services.Authentication
{
    public class CookieAuthenticationService : ICookieAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieAuthenticationService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
                throw new UserAlreadyRegisteredException($"User with email = {email} is already registered");

            _dbContext.Users.Add(new User { Email = email, PasswordHash = password });

            await _dbContext.SaveChangesAsync();

            await SignIn(email);
        }

        public async Task Login(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
                throw new NotFoundException($"User with email = {email} does not exist");

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
