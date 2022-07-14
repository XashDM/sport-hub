using SportHub.Domain.Models;
using System.Collections.Generic;

namespace SportHub.Services
{
    public interface IUserService
    {
        User GetUserByEmail(string email);

        User ChangePassword(string email, string passwordHash);

        bool IsExistingEmail(string email);
        IList<User> GetAllUsersList();

        IList<User> GetAllAdminsList();

        User CreateUser(string email, string? passwordHash, string firstName, string lastName, string? authProvider = null, bool isExternal = false);
    }
}