using System.Linq;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Exceptions.UserServiceExceptions;
using Microsoft.EntityFrameworkCore;

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
            var user = _context.Users.Include(u => u.Roles)
                                     .Where(u => u.Email.Equals(email))
                                     .First();

            return user;
        }

        public User CreateUser(string email, string passwordHash, string firstName, string lastName)
        {
            if (IsExistingEmail(email))
            {
                throw new EmailAlreadyInUseException();
            }

            var userRole = GetUserRoleByName("User");

            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Roles = userRole,
                IsActive = true
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
    }
}
