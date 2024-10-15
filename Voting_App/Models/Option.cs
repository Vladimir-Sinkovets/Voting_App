namespace Voting_App.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }
}
