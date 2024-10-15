namespace Voting_App.Services.PasswordHash
{
    public interface IPasswordHasher
    {
        string GetPasswordHash(string password);
        bool VerifyPasswordHash(string password, string passwordHash);
    }
}