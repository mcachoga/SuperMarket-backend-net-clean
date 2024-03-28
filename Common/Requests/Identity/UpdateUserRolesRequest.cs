using SuperMarket.Common.Responses.Identity;

namespace SuperMarket.Common.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }

        public List<UserRoleViewModel> Roles { get; set; }
    }
}