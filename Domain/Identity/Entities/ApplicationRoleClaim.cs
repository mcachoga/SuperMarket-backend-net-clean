using Microsoft.AspNetCore.Identity;

namespace SuperMarket.Domain.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string Description { get; set; }

        public string Group { get; set; }
    }
}