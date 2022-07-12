using SportHub.Domain.Models;
using SportHub.Domain.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services
{
    public interface IUserService
    {
        User CreateUser(string email, string passwordHash, string firstName, string lastName);

        User GetUserByEmail(string email);

        User ChangePassword(string email, string passwordHash);

        bool IsExistingEmail(string email);
        Task<IList<User>> GetAllUsersList();

        Task<IList<User>> GetAllAdminsList();

        Task<bool> BlockUserByIdAsync(int userId);

        Task<bool> ActivateUserByIdAsync(int userId);

        Task<bool> DeleteUserByIdAsync(int userId);

        Task<bool> GrantAdminRoleByIdAsync(int userId);

        Task<bool> RemoveAdminRoleByIdAsync(int userId);
    }
}