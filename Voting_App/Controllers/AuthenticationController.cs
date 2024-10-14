using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voting_App.Models;
using Voting_App.ViewModels;
using Voting_App.Services.Authentication;

namespace Voting_App.Controllers
{
    public class AuthenticationController : Controller
    {
        private List<User> _users = new List<User>();
        private readonly ICookieAuthenticationService _authenticationService;

        public AuthenticationController(ICookieAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var isSucceed = await _authenticationService.TryRegisterUserAsync(viewModel.Email, viewModel.Password);

            if (isSucceed)
                return RedirectToAction("Index", "Home");
            else
                return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var isSucceed = await _authenticationService.TryLogin(viewModel.Email, viewModel.Password);

            if (isSucceed == true)
                return RedirectToAction("Index", "Home");
            else
                return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();

            return RedirectToAction(nameof(Login));
        }
    }
}
