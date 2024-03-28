using SuperMarket.Common.Responses.Identity;

namespace SuperMarket.Common.Requests.Identity
{
    public class UpdateRolePermissionsRequest
    {
        public string RoleId { get; set; }

        public List<RoleClaimViewModel> RoleClaims { get; set; }
    }
}