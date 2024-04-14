using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Security;

namespace SuperMarket.Persistence.Identity.Context
{
    public class ApplicationIdentityDbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationIdentityDbContext _dbContext;

        public ApplicationIdentityDbSeeder(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            ApplicationIdentityDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task ConfigureDatabaseAsync(bool useSeedData = true)
        {
            await CheckAndApplyPendingMigrationAsync();
            
            if (useSeedData)
            {
                await SeIdentityDataAsync();
            }
        }

        private async Task CheckAndApplyPendingMigrationAsync()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        private async Task SeIdentityDataAsync()
        {
            try
            {
                if (await _dbContext.Users.AnyAsync() || await _dbContext.Roles.AnyAsync()) return;

                await SeedRolesAsync();
                await SeedBasicUserAsync();
                await SeedAdminUserAsync();
            }
            catch (Exception exception)
            {
                throw new Exception("An error occurred while migrating or seeding the database.", exception);
            }
        }

        private async Task SeedAdminUserAsync()
        {
            string adminUserName = AppCredentials.AdminEmail[..AppCredentials.AdminEmail.IndexOf('@')].ToLowerInvariant();
            var adminUser = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Root",
                Email = AppCredentials.AdminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = AppCredentials.AdminEmail.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true
            };

            if (!await _userManager.Users.AnyAsync(u => u.Email == AppCredentials.AdminEmail))
            {
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, AppCredentials.Password);
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, AppRoles.Basic)
                && !await _userManager.IsInRoleAsync(adminUser, AppRoles.Admin))
            {
                await _userManager.AddToRolesAsync(adminUser, AppRoles.DefaultRoles);
            }
        }

        private async Task SeedBasicUserAsync()
        {
            var basicUser = new ApplicationUser
            {
                FirstName = "Peter",
                LastName = "Reader",
                Email = AppCredentials.ReaderEmail,
                UserName = "peter",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = AppCredentials.ReaderEmail.ToUpperInvariant(),
                NormalizedUserName = "PETER",
                IsActive = true
            };

            if (!await _userManager.Users.AnyAsync(u => u.Email == "johnd@abc.com"))
            {
                var password = new PasswordHasher<ApplicationUser>();
                basicUser.PasswordHash = password.HashPassword(basicUser, AppCredentials.Password);
                await _userManager.CreateAsync(basicUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(basicUser, AppRoles.Basic))
            {
                await _userManager.AddToRoleAsync(basicUser, AppRoles.Basic);
            }
        }

        private async Task SeedRolesAsync()
        {
            foreach (var roleName in AppRoles.DefaultRoles)
            {
                if (await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName) is not ApplicationRole role)
                {
                    role = new ApplicationRole
                    {
                        Name = roleName,
                        Description = $"{roleName} Role."
                    };

                    await _roleManager.CreateAsync(role);
                }

                if (roleName == AppRoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(role, AppPermissions.AdminPermissions);
                }
                else if (roleName == AppRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(role, AppPermissions.BasicPermissions);
                }
            }
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationRole role, IReadOnlyList<AppPermission> permissions)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(claim => claim.Type == AppClaim.Permission && claim.Value == permission.Name))
                {
                    await _dbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = AppClaim.Permission,
                        ClaimValue = permission.Name,
                        Description= permission.Description,
                        Group = permission.Group
                    });

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}