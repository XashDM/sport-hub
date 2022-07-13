﻿using System.Linq;
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

        public User CreateUser(string email, string? passwordHash, string firstName, string lastName, bool isExternal = false)
        {
            if (IsExistingEmail(email))
            {
                throw new EmailAlreadyInUseException();
            }

            var userRole = GetUserRoleByName("User");
            bool isExternalUser = false;

            if (isExternal)
            {
                passwordHash = null;
                isExternalUser = true;
            }

            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Roles = userRole,
                IsActive = true,
                IsExternal = isExternalUser
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

        public async Task<IList<User>> GetAllUsersList()
        {
            var adminRole = GetUserRoleByName("Admin");
            var users = await _context.Users
                .Where(user => !user.Roles.Contains(adminRole[0]))
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
               .ToListAsync();

            var admins = adminRole[0].Users
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
            var adminRole = GetUserRoleByName("Admin");
            var userToBlock = await _context.Users
                .Where(user => user.Id == userId)
                .Include(user => user.Roles)
                .FirstOrDefaultAsync();
            if (userToBlock is not null)
            {
                //we can block only active user, if it's not an admin
                if (userToBlock.IsActive == true && !userToBlock.Roles.Contains(adminRole[0]))
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
                //check if userToGrantAdmin is active
                if (userToGrantAdmin.IsActive)
                {
                    var adminRole = GetUserRoleByName("Admin");
                    if (userToGrantAdmin.Roles.Contains(adminRole[0]))
                    {
                        return false;
                    }
                    else
                    {
                        userToGrantAdmin.Roles.Add(adminRole[0]);
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
                var adminRole = GetUserRoleByName("Admin");
                if (userToGrantAdmin.Roles.Contains(adminRole[0]))
                {
                    userToGrantAdmin.Roles.Remove(adminRole[0]);
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
