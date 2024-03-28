using Microsoft.AspNetCore.Identity;

namespace SuperMarket.Infrastructure.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}