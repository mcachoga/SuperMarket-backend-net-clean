﻿using Microsoft.AspNetCore.Http;
using SuperMarket.Application.Services.Identity;
using System.Security.Claims;

namespace SuperMarket.Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId { get; } = string.Empty;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        {
            UserId = string.Empty;

            try
            {
                // UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null)
                {
                    var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        UserId = claim.Value;
                    }
                }
            }
            catch 
            { 

            }
        }
    }
}