using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Identity.Roles.Commands;
using SuperMarket.Application.Features.Identity.Roles.Queries;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    public class RoleController : BaseGenericController<RoleController>
    {
        [HttpPost]
        [MustHavePermission(AppFeature.Roles, AppAction.Create)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var response = await MediatorSender.Send(new CreateRoleCommand { CreateRole = request });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("all")]
        [MustHavePermission(AppFeature.Roles, AppAction.Read)]
        public async Task<IActionResult> GetRoles()
        {
            var response = await MediatorSender.Send(new GetRolesQuery());
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut]
        [MustHavePermission(AppFeature.Roles, AppAction.Update)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest updateRole)
        {
            var response = await MediatorSender.Send(new UpdateRoleCommand { UpdateRole = updateRole });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{roleId}")]
        [MustHavePermission(AppFeature.Roles, AppAction.Read)]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var response = await MediatorSender.Send(new GetRoleByIdQuery { RoleId = roleId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("{roleId}")]        
        [MustHavePermission(AppFeature.Roles, AppAction.Delete)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var response = await MediatorSender.Send(new DeleteRoleCommand { RoleId = roleId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("permissions/{roleId}")]
        [MustHavePermission(AppFeature.RoleClaims, AppAction.Read)]
        public async Task<IActionResult> GetPermissions(string roleId)
        {
            var response = await MediatorSender.Send(new GetPermissionsQuery { RoleId = roleId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("update-permissions")]
        [MustHavePermission(AppFeature.RoleClaims, AppAction.Update)]
        public async Task<IActionResult> UpdateRolePermissions([FromBody] UpdateRolePermissionsRequest request)
        {
            var response = await MediatorSender.Send(new UpdateRolePermissionsCommand { UpdateRolePermissions = request });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}