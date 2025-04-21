using Cantina.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cantina.Application.Interface
{
    public interface IUserManager
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    }
}
