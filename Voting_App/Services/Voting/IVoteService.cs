namespace Voting_App.Services.Voting
{
    public interface IVoteService
    {
        IEnumerable<OptionData> GetOptionsData();
        bool HasUserVoted(string email);
        Task VoteAsync(string userEmail, int chosenOptionId);
    }
}