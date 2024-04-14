using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain.Identity;

namespace SuperMarket.Persistence.Identity.Context
{
    public class ApplicationIdentityDbContext 
        : IdentityDbContext<ApplicationUser, 
                            ApplicationRole, 
                            string,
                            IdentityUserClaim<string>,
                            IdentityUserRole<string>,
                            IdentityUserLogin<string>,
                            ApplicationRoleClaim,
                            IdentityUserToken<string>>
    {

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}