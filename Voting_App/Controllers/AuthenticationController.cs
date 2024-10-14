using Microsoft.AspNetCore.Mvc;
using Voting_App.ViewModels;
using Voting_App.Services.Authentication;

namespace Voting_App.Controllers
{
    public class AuthenticationController : Controller
    {
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
            if (ModelState.IsValid)
                return View(viewModel);

            var isSucceed = await _authenticationService.TryRegisterUserAsync(viewModel.Email, viewModel.Password);

            if (isSucceed)
                return RedirectToStartPage();
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
            if (ModelState.IsValid)
                return View(viewModel);

            var isSucceed = await _authenticationService.TryLogin(viewModel.Email, viewModel.Password);

            if (isSucceed == true)
                return RedirectToStartPage();
            else
                return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();

            return RedirectToStartPage();
        }

        private IActionResult RedirectToStartPage() => RedirectToAction("Index", "Home");
    }
}
