using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Identity.Users.Commands;
using SuperMarket.Application.Features.Identity.Users.Queries;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class UserController : BaseGenericController<UserController>
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
        {
            var response = await MediatorSender.Send(new UserRegistrationCommand { UserRegistration = userRegistration });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{userId}")]
        [MustHavePermission(AppFeature.Users, AppAction.Read)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var response = await MediatorSender.Send(new GetUserByIdQuery { UserId = userId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("all")]
        [MustHavePermission(AppFeature.Users, AppAction.Read)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await MediatorSender.Send(new GetAllUsersQuery());
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }

        [HttpPut]
        [MustHavePermission(AppFeature.Users, AppAction.Update)]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserRequest updateUser)
        {
            var response = await MediatorSender.Send(new UpdateUserCommand { UpdateUser = updateUser });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }

        [HttpPut("change-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest changePassword)
        {
            var response = await MediatorSender.Send(new ChangeUserPasswordCommand { ChangePassword = changePassword });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("change-status")]
        [MustHavePermission(AppFeature.Users, AppAction.Update)]
        public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusRequest changeUserStatus)
        {
            var response = await MediatorSender.Send(new ChangeUserStatusCommand { ChangeUserStatus = changeUserStatus });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            
            return NotFound(response);
        }

        [HttpGet("roles/{userId}")]
        [MustHavePermission(AppFeature.Roles, AppAction.Read)]
        public async Task<IActionResult> GetRoles(string userId)
        {
            var response = await MediatorSender.Send(new GetRolesQuery { UserId = userId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("user-roles")]
        [MustHavePermission(AppFeature.Users, AppAction.Update)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesRequest updateUserRoles)
        {
            var response = await MediatorSender.Send(new UpdateUserRolesCommand { UpdateUserRoles = updateUserRoles });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}