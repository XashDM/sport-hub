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

        public User CreateUser(string email, string? passwordHash, string firstName, string lastName, string? authProvider = null, bool isExternal = false)
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

            _context.Users.Add(user);
            _context.SaveChanges();

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

        public User ChangePassword(string email, string passwordHash)
        {
            var user = GetUserByEmail(email);
            user.PasswordHash = passwordHash;
            _context.SaveChanges();

            return user;
        }

        public IList<User> GetAllUsersList()
        {
            var users = _context.Users
                .Include(user => user.Roles)
                .ToList();
            return users;        
        }

        public IList<User> GetAllAdminsList()
        {
            var adminRole = _context.UserRoles
               .Where(role => role.RoleName == "Admin")
               .Include(role => role.Users)
               .ToList();

            var admins = adminRole[0].Users.ToList();

            return admins;
        }
    }
}
