using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Shared.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }

        public List<UserRoleViewModel> Roles { get; set; }
    }
}