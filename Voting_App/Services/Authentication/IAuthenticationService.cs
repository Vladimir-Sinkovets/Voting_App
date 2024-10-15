namespace Voting_App.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task Logout();
        Task Login(string email, string password);
        Task RegisterUserAsync(string username, string password);
    }
}