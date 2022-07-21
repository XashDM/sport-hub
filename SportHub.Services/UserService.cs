using System.Linq;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Exceptions.UserServiceExceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services
{
    public class UserService : IUserService
    {
        private readonly SportHubDBContext _context;

        public UserService(SportHubDBContext context)
        {
            _context = context;
        }

        public User GetUserByEmail(string email)
        {
            if (IsExistingEmail(email) == false)
            {
                throw new UserDoesNotExistException();
            }

            var user = _context.Users.Include(u => u.Roles)
                                     .Where(u => u.Email.Equals(email))
                                     .First();

            return user;
        }

        public async Task<User> CreateUser(string email, string? passwordHash, string firstName, string lastName, string? authProvider = null, bool isExternal = false)
        {
            if (IsExistingEmail(email))
            {
                throw new EmailAlreadyInUseException();
            }

            var userRole = GetUserRoleByName("User");

            var user = new User
            {
                Email = email,
                PasswordHash = isExternal ? null : passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Roles = userRole,
                IsActive = true,
                IsExternal = isExternal,
                AuthProvider = authProvider
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public bool IsExistingEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).Any();
        }

        private UserRole[] GetUserRoleByName(string roleName)
        { 
            var userRole = _context.UserRoles
                .Where(r => r.RoleName == roleName)
                .ToArray();
           
            return userRole;
        }

        private async Task<UserRole> GetSingleUserRoleByName(string roleName)
        {
            var userRole = await _context.UserRoles
                .Where(r => r.RoleName == roleName)
                .FirstOrDefaultAsync();

            return userRole;
        }

        public User ChangePassword(string email, string passwordHash)
        {
            var user = GetUserByEmail(email);
            user.PasswordHash = passwordHash;
            _context.SaveChanges();

            return user;
        }

        public async Task<IList<User>> GetAllUsersList()
        {
            var adminRole = await GetSingleUserRoleByName("Admin");

            var users = await _context.Users
                .Where(user => !user.Roles.Contains(adminRole))
                .Select(user => new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = user.Roles
                })
                .ToListAsync();

            return users;        
        }

        public async Task<IList<User>> GetAllAdminsList()
        {
            var adminRole = await _context.UserRoles
               .Where(role => role.RoleName == "Admin")
               .Include(role => role.Users)
               .FirstOrDefaultAsync();

            var admins = adminRole.Users
                .Select(user => new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = user.Roles
                })
                .ToList();

            return admins;
        }

        public async Task<bool> BlockUserByIdAsync(int userId)
        {
            var adminRole = await GetSingleUserRoleByName("Admin");

            var userToBlock = await _context.Users
                .Where(user => user.Id == userId)
                .Include(user => user.Roles)
                .FirstOrDefaultAsync();
            if (userToBlock is not null)
            {
                //we can block only active user, if it's not an admin
                if (userToBlock.IsActive == true && !userToBlock.Roles.Contains(adminRole))
                {
                    userToBlock.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ActivateUserByIdAsync(int userId)
        {
            var userToActivate = await _context.Users.FindAsync(userId);
            //we dont't need checking for admin role, admin can't be blocked
            if (userToActivate is not null)
            {
                if (userToActivate.IsActive == false)
                {
                    userToActivate.IsActive = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);
            if (userToDelete is not null)
            {
                _context.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> GrantAdminRoleByIdAsync(int userId)
        {
            var userToGrantAdmin = await _context.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (userToGrantAdmin is not null)
            {
                if (userToGrantAdmin.IsActive)
                {
                    var adminRole = await GetSingleUserRoleByName("Admin");
                    if (userToGrantAdmin.Roles.Contains(adminRole))
                    {
                        return false;
                    }
                    else
                    {
                        userToGrantAdmin.Roles.Add(adminRole);
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RemoveAdminRoleByIdAsync(int userId)
        {
            var userToGrantAdmin = await _context.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (userToGrantAdmin is not null)
            {
                var adminRole = await GetSingleUserRoleByName("Admin");
                if (userToGrantAdmin.Roles.Contains(adminRole))
                {
                    userToGrantAdmin.Roles.Remove(adminRole);
                }
                else
                {
                    return false;
                }
                await _context.SaveChangesAsync();
                return true;

            }
            return false;
        }
    }
}
