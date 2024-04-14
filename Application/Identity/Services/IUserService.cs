using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Identity;

namespace SuperMarket.Application.Identity.Services.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> UpdateUserAsync(string userId, ApplicationUser user);

        Task<IdentityResult> UpdateUserRolesAsync(string userId, IList<string> rolesNamesToAssign);

        Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword);

        Task<IdentityResult> ChangeUserStatusAsync(string userId, bool setToActive);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task<ApplicationUser> GetUserByNameAsync(string name);

        Task<IList<ApplicationUser>> GetAllUsersAsync();

        Task<IList<ApplicationRole>> GetUserRolesAsync(string userId);
    }
}