﻿using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Services.Identity
{
    public interface IUserService
    {
        Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request);

        Task<IResponseWrapper> GetUserByIdAsync(string userId);

        Task<IResponseWrapper> GetAllUsersAsync();

        Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request);

        Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest request);

        Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request);

        Task<IResponseWrapper> GetRolesAsync(string userId);

        Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRolesRequest request);   
        
        Task<IResponseWrapper<UserResponse>> GetUserByEmailAsync(string email);
    }
}