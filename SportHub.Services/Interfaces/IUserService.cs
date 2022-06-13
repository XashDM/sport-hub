using SportHub.Domain.Models;

namespace SportHub.Services
{
    public interface IUserService
    {
        User CreateUser(string email, string passwordHash, string firstName, string lastName);

        User GetUserByEmail(string email);

        User ChangePassword(string email, string passwordHash);

        bool IsExistingEmail(string email);
    }
}