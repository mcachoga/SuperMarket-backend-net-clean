﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;
using SuperMarket.Infrastructure.Models;

namespace SuperMarket.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IMapper mapper, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IResponseWrapper> GetUserByIdAsync(string userId)
        {
            var userInDb = await _userManager.FindByIdAsync(userId);
            if (userInDb is not null)
            {
                // Mapper - AutoMapper
                var mappedUser = _mapper.Map<UserResponse>(userInDb);
                return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail is not null)
            {
                return await ResponseWrapper.FailAsync("Email already taken.");
            }

            var userWithSameUsername = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUsername is not null)
            {
                return await ResponseWrapper.FailAsync("Username already taken.");
            }

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoComfirmEmail,
            };

            // Hash and store the Hash password
            var password = new PasswordHasher<ApplicationUser>();
            newUser.PasswordHash = password.HashPassword(newUser, request.Password);

            var identityResult = await _userManager.CreateAsync(newUser);

            if (identityResult.Succeeded)
            {
                // Assign to Basic Role
                await _userManager.AddToRoleAsync(newUser, AppRoles.Basic);

                return await ResponseWrapper<string>.SuccessAsync("User registered successfully.");
            }
            return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
        }

        public async Task<IResponseWrapper> GetAllUsersAsync()
        {
            var usersInDb = await _userManager
                .Users
                .ToListAsync();

            if (usersInDb.Count > 0)
            {
                var mappedUsers = _mapper.Map<List<UserResponse>>(usersInDb);
                return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
            }
            return await ResponseWrapper.FailAsync("No Users were found.");
        }

        public async Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request)
        {
            var userInDb = await _userManager.FindByIdAsync(request.UserId);
            if (userInDb is not null)
            {
                userInDb.FirstName = request.FirstName;
                userInDb.LastName = request.LastName;
                userInDb.PhoneNumber = request.PhoneNumber;

                var identityResult = await _userManager.UpdateAsync(userInDb);
                if (identityResult.Succeeded)
                {
                    return await ResponseWrapper<string>.SuccessAsync("User details successfully updated.");
                }
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        public async Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest request)
        {
            var userInDb = await _userManager.FindByIdAsync(request.UserId);
            if (userInDb is not null)
            {
                var identityResult = await _userManager.ChangePasswordAsync(
                    userInDb,
                    request.CurrentPassword,
                    request.NewPassword);
                if (identityResult.Succeeded)
                {
                    return await ResponseWrapper<string>.SuccessAsync("User password updated.");
                }
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request)
        {
            var userInDb = await _userManager.FindByIdAsync(request.UserId);
            if (userInDb is not null)
            {
                // Change status
                userInDb.IsActive = request.Activate;
                var identityResult = await _userManager.UpdateAsync(userInDb);

                if (identityResult.Succeeded)
                {
                    return await ResponseWrapper<string>
                        .SuccessAsync(request.Activate ? "User actived successfully."
                            : "User de-activated successfully");
                }
                return await ResponseWrapper
                    .FailAsync(GetIdentityResultErrorDescriptions(identityResult));
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }
            return errorDescriptions;
        }

        public async Task<IResponseWrapper> GetRolesAsync(string userId)
        {
            var userRolesVM = new List<UserRoleViewModel>();
            var userIdDb = await _userManager.FindByIdAsync(userId);
            if (userIdDb is not null)
            {
                // Get roles
                var allRoles = await _roleManager.Roles.ToListAsync();
                foreach (var role in allRoles)
                {
                    var userRoleVM = new UserRoleViewModel
                    {
                        RoleName = role.Name,
                        RoleDescription = role.Description
                    };

                    if (await _userManager.IsInRoleAsync(userIdDb, role.Name))
                    {
                        // User is assigned this role.
                        userRoleVM.IsAssignedToUser = true;
                    }
                    else
                    {
                        userRoleVM.IsAssignedToUser = false;
                    }

                    userRolesVM.Add(userRoleVM);
                }

                return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRolesVM);
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        public async Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRolesRequest request)
        {
            var userInDb = await _userManager.FindByIdAsync(request.UserId);
            if (userInDb is not null)
            {
                if (userInDb.Email == AppCredentials.AdminEmail)
                {
                    return await ResponseWrapper.FailAsync("User Roles update not permitted.");
                }
                var currentAssignedRoles = await _userManager.GetRolesAsync(userInDb);
                var rolesToBeAssigned = request.Roles
                    .Where(role => role.IsAssignedToUser == true)
                    .ToList();

                var currentLoggedInUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                if (currentLoggedInUser is null)
                {
                    return await ResponseWrapper.FailAsync("User does not exist.");
                }

                if (await _userManager.IsInRoleAsync(currentLoggedInUser, AppRoles.Admin))
                {
                    var identityResult1 = await _userManager.RemoveFromRolesAsync(userInDb, currentAssignedRoles);
                    if (identityResult1.Succeeded)
                    {
                        var identityResult2 = await _userManager
                            .AddToRolesAsync(userInDb, rolesToBeAssigned.Select(role => role.RoleName));
                        if (identityResult2.Succeeded)
                        {
                            return await ResponseWrapper<string>.SuccessAsync("User Roles Updated Successfully.");
                        }
                        return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult2));
                    }
                    return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult1));
                }
                return await ResponseWrapper.FailAsync("User Roles update not permitted.");
            }
            return await ResponseWrapper.FailAsync("User does not exist.");
        }

        public async Task<IResponseWrapper<UserResponse>> GetUserByEmailAsync(string email)
        {
            var userInDb = await _userManager.FindByEmailAsync(email);
            if (userInDb is not null)
            {
                var mappedUser = _mapper.Map<UserResponse>(userInDb);
                return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
            }
            return await ResponseWrapper<UserResponse>.FailAsync("User does not exist");
        }
    }
}
