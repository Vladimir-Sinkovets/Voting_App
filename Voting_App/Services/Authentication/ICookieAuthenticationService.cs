namespace Voting_App.Services.Authentication
{
    public interface ICookieAuthenticationService
    {
        Task Logout();
        Task<bool> TryLogin(string email, string password);
        Task<bool> TryRegisterUserAsync(string username, string password);
    }
}