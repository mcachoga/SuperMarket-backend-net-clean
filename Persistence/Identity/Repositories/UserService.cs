using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Security;

namespace SuperMarket.Persistence.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, 
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            // Estas validaciones nunca deberían de ocurrir, ya que se controlan en UserRegistrationRequestValidator.
            var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);
            if (userWithSameEmail is not null) throw new CustomAuthException(friendlyMessage: "Email already taken.");

            var userWithSameUsername = await _userManager.FindByNameAsync(user.UserName);
            if (userWithSameUsername is not null) throw new CustomAuthException(friendlyMessage: "Username already taken.");

            var newUser = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();
            newUser.PasswordHash = passwordHash.HashPassword(newUser, password);

            var identityResult = await _userManager.CreateAsync(newUser);

            if (identityResult.Succeeded)
            {
                return await _userManager.AddToRoleAsync(newUser, AppRoles.Basic);
            }

            return identityResult;
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, ApplicationUser user)
        {
            var userInDb = await GetUserByIdAsync(userId);

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.PhoneNumber = user.PhoneNumber;

            return await _userManager.UpdateAsync(userInDb);
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(string userId, IList<string> rolesNamesToAssign)
        {
            var userInDb = await GetUserByIdAsync(userId);

            if (userInDb.Email == AppCredentials.AdminEmail) 
                throw new CustomAuthException(friendlyMessage: "User Roles update not permitted.");
            
            var currentLoggedInUser = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (currentLoggedInUser is null)
                throw new CustomAuthException(friendlyMessage: "User does not exist.");

            if (await _userManager.IsInRoleAsync(currentLoggedInUser, AppRoles.Admin) == false)
                throw new CustomAuthException(friendlyMessage: "User Roles update not permitted.");

            var currentAssignedRoles = await _userManager.GetRolesAsync(userInDb);
            var identityResultRemove = await _userManager.RemoveFromRolesAsync(userInDb, currentAssignedRoles);
            
            if (!identityResultRemove.Succeeded) 
                return identityResultRemove;

            return await _userManager.AddToRolesAsync(userInDb, rolesNamesToAssign);
        }

        public async Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var userInDb = await GetUserByIdAsync(userId);
            
            return await _userManager.ChangePasswordAsync(userInDb, currentPassword, newPassword);
        }

        public async Task<IdentityResult> ChangeUserStatusAsync(string userId, bool setToActive)
        {
            var userInDb = await GetUserByIdAsync(userId);

            userInDb.IsActive = setToActive;
            
            return await _userManager.UpdateAsync(userInDb);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var userInDb = await _userManager.FindByIdAsync(userId) ??
                throw new CustomAuthException(friendlyMessage: "User does not exist.");

            return userInDb;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<IList<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IList<ApplicationRole>> GetUserRolesAsync(string userId)
        {
            var userInDb = await GetUserByIdAsync(userId);

            var userRoles = new List<ApplicationRole>();
            var allRoles = await _roleManager.Roles.ToListAsync();
            
            foreach (var role in allRoles)
            {
                if (await _userManager.IsInRoleAsync(userInDb, role.Name))
                {
                    userRoles.Add(role);
                }
            }

            return userRoles;
        }
    }
}