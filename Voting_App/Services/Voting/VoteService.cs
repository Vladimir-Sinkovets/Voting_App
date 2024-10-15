using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;

namespace Voting_App.Services.Voting
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _dbContext;

        public VoteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OptionData> GetOptionsData()
        {
            var options = _dbContext.Options;

            var optionsData = options.Select(o => new OptionData() { Id = o.Id, Name = o.Name, VoteCount = o.Votes.Count });

            return optionsData;
        }

        public bool HasUserVoted(string email)
        {
            var vote = _dbContext.Votes.FirstOrDefault(v => v.User.Email == email);

            return vote != null;
        }

        public async Task VoteAsync(string userEmail, int chosenOptionId)
        {
            var userVote = _dbContext.Votes.FirstOrDefault(v => v.User.Email == userEmail);
            var option = _dbContext.Options.FirstOrDefault(o => o.Id == chosenOptionId);
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (userVote != null || option == null || user == null)
                throw new Exception(); // todo: add specific exceptions


            var newVote = new Vote() { Option = option, User = user };

            _dbContext.Votes.Add(newVote);

            await _dbContext.SaveChangesAsync();
        }
    }
}
