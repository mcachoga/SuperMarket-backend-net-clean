using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Shared.Requests.Identity
{
    public class UpdateRolePermissionsRequest
    {
        public string RoleId { get; set; }

        public List<RoleClaimViewModel> RoleClaims { get; set; }
    }
}