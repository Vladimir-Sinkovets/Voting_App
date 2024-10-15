using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;
using Voting_App.Services.Exceptions;

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

        public bool HasUserVoted(string userEmail)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail)
                ?? throw new NotFoundException($"User with email = {userEmail} does not exist");

            var vote = _dbContext.Votes.FirstOrDefault(v => v.User.Email == userEmail);

            return vote != null;
        }

        public async Task VoteAsync(string userEmail, int chosenOptionId)
        {
            var option = _dbContext.Options.FirstOrDefault(o => o.Id == chosenOptionId) 
                ?? throw new NotFoundException($"Option with id = {chosenOptionId} does not exist");

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail)
                ?? throw new NotFoundException($"User with email = {userEmail} does not exist");
            
            var userVote = _dbContext.Votes.FirstOrDefault(v => v.User.Email == userEmail);

            if (userVote != null)
                throw new UserAlreadyVotedException($"User with email = {userEmail} has already voted");


            var newVote = new Vote() { Option = option, User = user };

            _dbContext.Votes.Add(newVote);

            await _dbContext.SaveChangesAsync();
        }
    }
}
