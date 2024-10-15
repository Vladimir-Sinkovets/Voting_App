namespace Voting_App.Services.Authentication
{
    public interface ICookieAuthenticationService
    {
        Task Logout();
        Task Login(string email, string password);
        Task RegisterUserAsync(string username, string password);
    }
}