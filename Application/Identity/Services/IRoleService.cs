using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Identity;

namespace SuperMarket.Application.Identity.Services.Contracts
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);

        Task<IdentityResult> UpdateRoleAsync(string roleId, ApplicationRole role);

        Task<IdentityResult> DeleteRoleAsync(string roleId);

        Task UpdateRolePermissionsAsync(string roleId, IList<ApplicationRoleClaim> roleClaimsToAssign);

        Task<IList<ApplicationRole>> GetRolesAsync();

        Task<ApplicationRole> GetRoleByIdAsync(string roleId);

        Task<IList<ApplicationRoleClaim>> GetAllClaimsForRoleAsync(string roleId);
    }
}