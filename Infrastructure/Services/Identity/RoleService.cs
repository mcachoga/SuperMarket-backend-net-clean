﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;
using SuperMarket.Infrastructure.Context;
using SuperMarket.Infrastructure.Models;

namespace SuperMarket.Infrastructure.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper, 
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest request)
        {
            var roleExist = await _roleManager.FindByNameAsync(request.RoleName);
            if (roleExist is not null)
            {
                return await ResponseWrapper<string>.FailAsync("Role already exists");
            }

            var newRole = new ApplicationRole
            {
                Name = request.RoleName,
                Description = request.RoleDescription
            };

            var identityResult = await _roleManager.CreateAsync(newRole);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("Role created successfully");
            }

            return await ResponseWrapper<string>
                .FailAsync(GetIdentityResultErrorDescriptions(identityResult));

        }

        public async Task<IResponseWrapper> GetRoleByIdAsync(string roleId)
        {
            var roleInDb = await _roleManager.FindByIdAsync(roleId);
            if (roleInDb is not null)
            {
                var mappedRole = _mapper.Map<RoleResponse>(roleInDb);
                return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
            }
            return await ResponseWrapper.FailAsync("Role does not exist.");
        }

        public async Task<IResponseWrapper> GetRolesAsync()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            if (allRoles.Count > 0)
            {
                var mappedRoles = _mapper.Map<List<RoleResponse>>(allRoles);
                return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
            }
            return await ResponseWrapper<string>.FailAsync("No roles were found.");
        }

        public async Task<IResponseWrapper> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var roleInDb = await _roleManager.FindByIdAsync(request.RoleId);
            if (roleInDb is not null)
            {
                if (roleInDb.Name != AppRoles.Admin)
                {
                    roleInDb.Name = request.RoleName;
                    roleInDb.Description = request.RoleDescription;

                    var identityResult = await _roleManager.UpdateAsync(roleInDb);
                    if (identityResult.Succeeded)
                    {
                        return await ResponseWrapper<string>
                            .SuccessAsync("Role updated successfully");
                    }
                    return await ResponseWrapper
                        .FailAsync(GetIdentityResultErrorDescriptions(identityResult));

                }
                return await ResponseWrapper.FailAsync("Cannot update Admin role.");
            }
            return await ResponseWrapper.FailAsync("Role does not exist.");
        }

        public async Task<IResponseWrapper> DeleteRoleAsync(string roleId)
        {
            var roleInDb = await _roleManager.FindByIdAsync(roleId);
            if (roleInDb is not null)
            {
                if (roleInDb.Name != AppRoles.Admin)
                {
                    var allUsers = await _userManager.Users.ToListAsync();
                    foreach (var user in allUsers)
                    {
                        if (await _userManager.IsInRoleAsync(user, roleInDb.Name))
                        {
                            return await ResponseWrapper
                                .FailAsync($"Role: {roleInDb.Name} is currently assigned to a user.");
                        }
                    }

                    var identityResult = await _roleManager.DeleteAsync(roleInDb);
                    if (identityResult.Succeeded)
                    {
                        return await ResponseWrapper.SuccessAsync("Role successfully deleted.");
                    }
                    return await ResponseWrapper
                        .FailAsync(GetIdentityResultErrorDescriptions(identityResult));
                }
                return await ResponseWrapper.FailAsync("Cannot delete Admin role.");
            }
            return await ResponseWrapper.FailAsync("Role does not exist.");
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

        public async Task<IResponseWrapper> GetPermissionsAsync(string roleId)
        {
            var roleInDb = await _roleManager.FindByIdAsync(roleId);
            if (roleInDb is not null)
            {
                var allPermissions = AppPermissions.AllPermissions;
                var roleClaimResponse = new RoleClaimResponse
                {
                    Role = new()
                    {
                        Id = roleId,
                        Name = roleInDb.Name,
                        Description = roleInDb.Description,
                    },
                    RoleClaims = new()
                };

                var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);

                var allPermissionsNames = allPermissions.Select(p => p.Name).ToList();
                var currentRoleClaimsValues = currentRoleClaims.Select(crc => crc.ClaimValue).ToList();

                var currentlyAssignedRoleClaimsNames = allPermissionsNames
                    .Intersect(currentRoleClaimsValues)
                    .ToList();

                foreach (var permission in allPermissions)
                {
                    if (currentlyAssignedRoleClaimsNames.Any(carc => carc == permission.Name))
                    {
                        roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel
                        {
                            RoleId = roleId,
                            ClaimType = AppClaim.Permission,
                            ClaimValue = permission.Name,
                            Description = permission.Description,
                            Group = permission.Group,
                            IsAssignedToRole = true
                        });
                    }
                    else
                    {
                        roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel
                        {
                            RoleId = roleId,
                            ClaimType = AppClaim.Permission,
                            ClaimValue = permission.Name,
                            Description = permission.Description,
                            Group = permission.Group,
                            IsAssignedToRole = false
                        });
                    }
                }
                return await ResponseWrapper<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
            }
            return await ResponseWrapper<RoleClaimResponse>.FailAsync("Role does not exist.");
        }

        private async Task<List<RoleClaimViewModel>> GetAllClaimsForRoleAsync(string roleId)
        {
            var roleClaims = await _context.RoleClaims
                .Where(rc => rc.RoleId == roleId)
                .ToListAsync();
            if (roleClaims.Count > 0)
            {
                var mappedRoleClaims = _mapper.Map<List<RoleClaimViewModel>>(roleClaims);
                return mappedRoleClaims;
            }
            return new List<RoleClaimViewModel>();
        }

        public async Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            var roleInDb = await _roleManager.FindByIdAsync(request.RoleId);
            if (roleInDb is not null)
            {
                if (roleInDb.Name == AppRoles.Admin)
                {
                    return await ResponseWrapper<string>.FailAsync("Cannot change permissions for this role.");
                }

                var permissionsToBeAssigned = request.RoleClaims
                    .Where(rc => rc.IsAssignedToRole == true)
                    .ToList();

                var currentlyAssignedClaims = await _roleManager.GetClaimsAsync(roleInDb);

                // Drop
                foreach (var claim in currentlyAssignedClaims)
                {
                    await _roleManager.RemoveClaimAsync(roleInDb, claim);
                }

                // Add
                foreach (var claim in permissionsToBeAssigned)
                {
                    var mappedRoleClaim = _mapper.Map<ApplicationRoleClaim>(claim);
                    await _context.RoleClaims.AddAsync(mappedRoleClaim);
                }
                await _context.SaveChangesAsync();
                return await ResponseWrapper<string>.SuccessAsync("Role permissions updated successfully.");
            }
            return await ResponseWrapper.FailAsync("Role does not exist.");
        }
    }
}