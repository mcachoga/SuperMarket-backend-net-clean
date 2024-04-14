using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Shared.Requests.Identity
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }
    }
}