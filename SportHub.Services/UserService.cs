using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Exceptions.UserServiceExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private bool isExistingEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).Any();
        }

        public User CreateUser(string email, string passwordHash, string firstName, string lastName)
        {
            if (isExistingEmail(email))
            {
                throw new EmailAlreadyInUseException();
            }

            var userRole = _context.UserRoles
                .Where(r => r.RoleName == "User")
                .ToArray();

            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Roles = userRole
            };


            _context.Users.Add(user);
            _context.SaveChanges();

            return user;

        }
    }
}
