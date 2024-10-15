namespace Voting_App.Services.Exceptions
{
    public class UserAlreadyVotedException : Exception
    {
        public UserAlreadyVotedException (string message) : base(message) { }
    }
}
