using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;

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

        public async Task<bool> TryRegisterUserAsync(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
                return false;

            _dbContext.Users.Add(new User { Email = email, PasswordHash = password });

            await _dbContext.SaveChangesAsync();

            await SignIn(email);

            return true;
        }

        public async Task<bool> TryLogin(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
                return false;

            await SignIn(email);

            return true;
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
