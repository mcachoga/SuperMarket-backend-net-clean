using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Shared.Requests.Identity
{
    public class UpdateRoleRequest
    {
        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }
    }
}