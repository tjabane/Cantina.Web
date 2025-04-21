using Cantina.Application.Interface;
using Cantina.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cantina.Infrastructure.Authentication
{
    public class UserManagerWrapper(UserManager<ApplicationUser> userManager) : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
