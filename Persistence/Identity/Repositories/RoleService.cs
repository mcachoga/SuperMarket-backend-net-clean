using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Persistence.Identity.Context;

namespace SuperMarket.Persistence.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationIdentityDbContext _context;

        public RoleService(
            RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            ApplicationIdentityDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            if (role is null || string.IsNullOrEmpty(role.Name))
            {
                throw new CustomAuthException(friendlyMessage: "Role not defined.");
            }

            var roleExist = await _roleManager.FindByNameAsync(role.Name);
            if (roleExist is not null)
            {
                throw new CustomAuthException(friendlyMessage: "Role already exists.");
            }

            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, ApplicationRole role)
        {
            var roleInDb = await GetRoleByIdAsync(roleId);
            
            if (roleInDb.Name == AppRoles.Admin)
            {
                throw new CustomAuthException(friendlyMessage: "Cannot update Admin role.");
            }

            roleInDb.Name = role.Name;
            roleInDb.Description = role.Description;

            return await _roleManager.UpdateAsync(roleInDb);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var roleInDb = await GetRoleByIdAsync(roleId);

            if (roleInDb.Name == AppRoles.Admin)
            {
                throw new CustomAuthException(friendlyMessage: "Cannot delete Admin role.");
            }

            var allUsers = await _userManager.Users.ToListAsync();
            
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, roleInDb.Name))
                {
                    throw new CustomAuthException(friendlyMessage: $"Role: {roleInDb.Name} is currently assigned to a user.");
                }
            }

            return await _roleManager.DeleteAsync(roleInDb);
        }

        public async Task UpdateRolePermissionsAsync(string roleId, IList<ApplicationRoleClaim> roleClaimsToAssign)
        {
            var roleInDb = await GetRoleByIdAsync(roleId);

            if (roleInDb.Name == AppRoles.Admin)
            {
                throw new CustomAuthException(friendlyMessage: "Cannot change permissions for this role.");
            }

            var currentlyAssignedClaims = await _roleManager.GetClaimsAsync(roleInDb);

            foreach (var claim in currentlyAssignedClaims)
            {
                await _roleManager.RemoveClaimAsync(roleInDb, claim);
            }

            foreach (var claim in roleClaimsToAssign)
            {
                await _context.RoleClaims.AddAsync(claim);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(string roleId)
        {
            var roleInDb = await _roleManager.FindByIdAsync(roleId) ??
                throw new CustomAuthException(friendlyMessage: "Role does not exist.");

            return roleInDb;
        }

        public async Task<IList<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IList<ApplicationRoleClaim>> GetAllClaimsForRoleAsync(string roleId)
        {
            var roleInDb = await GetRoleByIdAsync(roleId);

            var roleClaims = await _context.RoleClaims.Where(rc => rc.RoleId == roleId).ToListAsync();

            return roleClaims ?? new List<ApplicationRoleClaim>();
        }
    }
}