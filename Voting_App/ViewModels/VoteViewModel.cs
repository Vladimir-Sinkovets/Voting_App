using Voting_App.Services.Voting;

namespace Voting_App.ViewModels
{
    public class VoteViewModel
    {
        public IEnumerable<OptionData> OptionsData { get; set; }

        public bool HasUserVoted { get; set; }

        public int ChosenOptionId {  get; set; }
    }
}
