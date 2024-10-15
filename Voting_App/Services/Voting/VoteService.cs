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
            var user = _dbContext.Votes.FirstOrDefault(v => v.User.Email == email);

            return user != null;
        }
    }
}
