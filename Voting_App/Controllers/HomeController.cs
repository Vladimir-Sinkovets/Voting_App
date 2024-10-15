using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Voting_App.Models;
using Voting_App.Services.Voting;
using Voting_App.ViewModels;

namespace Voting_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVoteService _voteService;

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

            var hasUserVoted = _voteService.HasUserVoted(HttpContext.User.Identity!.Name!);

            var viewModel = new VoteViewModel()
            {
                OptionsData = options,
                HasUserVoted = hasUserVoted,
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
