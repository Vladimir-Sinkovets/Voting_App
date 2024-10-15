namespace Voting_App.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public User User{ get; set; }
        public Option Option { get; set; }
    }
}
