using Microsoft.AspNetCore.Identity;

namespace SuperMarket.Domain.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}