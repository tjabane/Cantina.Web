using Cantina.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Database.Data
{
    public class SeedUsersRoles
    {
        private readonly List<IdentityRole> _roles;
        private readonly List<ApplicationUser> _users;
        private readonly List<IdentityUserRole<string>> _userRoles;

        public SeedUsersRoles()
        {
            _roles = GetRoles();
            _users = GetUsers();
            _userRoles = GetUserRoles(_users, _roles);
        }

        public List<IdentityRole> Roles { get { return _roles; } }
        public List<ApplicationUser> Users { get { return _users; } }
        public List<IdentityUserRole<string>> UserRoles { get { return _userRoles; } }

        private List<IdentityRole> GetRoles()
        {
            var adminRole = new IdentityRole("Admin");
            adminRole.Id = "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc";
            adminRole.NormalizedName = adminRole.Name!.ToUpper();
            var memberRole = new IdentityRole("Member");
            memberRole.Id = "4901a6a2-7630-4573-a2c2-b98acbe3c276";
            memberRole.NormalizedName = memberRole.Name!.ToUpper();
            List<IdentityRole> roles = new() { adminRole, memberRole };
            return roles;
        }

        private List<ApplicationUser> GetUsers()
        {
            string pwd = "$400Project";
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            // Seed Users
            var adminUser = new ApplicationUser
            {
                Id = "e2d35ad5-21b1-40ff-976d-f38c4df46f2c",
                FullName = "Cantina Admin",
                UserName = "admin@cantina.com",
                Email = "admin@cantina.com",
                EmailConfirmed = true,
            };
            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

            List<ApplicationUser> users = new() { adminUser };
            return users;
        }

        private List<IdentityUserRole<string>> GetUserRoles(List<ApplicationUser> users, List<IdentityRole> roles)
        {
            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = "e2d35ad5-21b1-40ff-976d-f38c4df46f2c",
                RoleId = "0fb3a09d-320e-43b2-9a3f-bc1a3597aefc"
            });
            return userRoles;
        }
    }
}
