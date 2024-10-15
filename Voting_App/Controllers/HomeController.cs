using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Voting_App.Models;
using Voting_App.Services.Exceptions;
using Voting_App.Services.Voting;
using Voting_App.ViewModels;

namespace Voting_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVoteService _voteService;

        private string UserEmail { get => HttpContext.User.Identity!.Name!; }
        public HomeController(IVoteService voteService)
        {
            _voteService = voteService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Vote()
        {
            var options = _voteService.GetOptionsData();

            var hasUserVoted = _voteService.HasUserVoted(UserEmail);

            var viewModel = new VoteViewModel()
            {
                OptionsData = options,
                HasUserVoted = hasUserVoted,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(VoteViewModel viewModel)
        {
            try
            {
                await _voteService.VoteAsync(UserEmail, viewModel.ChosenOptionId);
            }
            catch (UserAlreadyVotedException)
            {
                return RedirectToAction(nameof(Vote));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch 
            {
                throw;
            }

            return RedirectToAction(nameof(Vote));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
