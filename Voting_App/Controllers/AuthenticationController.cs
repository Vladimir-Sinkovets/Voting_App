using Microsoft.AspNetCore.Mvc;
using Voting_App.ViewModels;
using Voting_App.Services.Authentication;
using Voting_App.Services.Exceptions;

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
            if (ModelState.IsValid == false)
                return View(viewModel);

            try
            {
                await _authenticationService.RegisterUserAsync(viewModel.Email, viewModel.Password);
            }
            catch (UserAlreadyRegisteredException)
            {
                return View(viewModel);
            }
            catch
            {
                throw;
            }

            return RedirectToStartPage();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid == false)
                return View(viewModel);

            try
            {
                await _authenticationService.Login(viewModel.Email, viewModel.Password);
            }
            catch (NotFoundException)
            {
                return View(viewModel);
            }
            catch
            {
                throw;
            }
            return RedirectToStartPage();
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();

            return RedirectToStartPage();
        }

        private IActionResult RedirectToStartPage() => RedirectToAction("Index", "Home");
    }
}
