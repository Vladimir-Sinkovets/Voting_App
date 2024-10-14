using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voting_App.Models;
using Voting_App.ViewModels;

namespace Voting_App.Controllers
{
    public class AuthenticationController : Controller
    {
        private List<User> _users = new List<User>();

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            User user = _users.FirstOrDefault(u => u.Email == viewModel.Email);

            if (user == null)
            {
                _users.Add(new User { Email = viewModel.Email, PasswordHash = viewModel.Password });

                await SignIn(viewModel.Email);

                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel viewModel)
        {
            User user = _users.FirstOrDefault(u => u.Email == viewModel.Email && u.PasswordHash == viewModel.Password);

            if (user != null)
            {
                _users.Add(new User { Email = viewModel.Email, PasswordHash = viewModel.Password });

                await SignIn(viewModel.Email);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Register));
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        private async Task SignIn(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
