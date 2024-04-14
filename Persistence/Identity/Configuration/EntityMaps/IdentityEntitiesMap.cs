using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Domain.Identity;

namespace SuperMarket.Persistence.Identity.Configuration
{
    internal class ApplicationUserEntityMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", SchemaNames.Security);
        }
    }

    internal class ApplicationRoleEntityMap : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("Roles", SchemaNames.Security);
        }
    }
    
    internal class ApplicationRoleEntityClaimMap : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("RoleClaims", SchemaNames.Security);
        }
    }
    
    internal class IdentityUserRoleEntityMap : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) =>
            builder.ToTable("UserRoles", SchemaNames.Security);
    }

    internal class IdentityUserClaimEntityMap : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder) =>
            builder.ToTable("UserClaims", SchemaNames.Security);
    }

    internal class IdentityUserLoginEntityMap : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder) =>
            builder.ToTable("UserLogins", SchemaNames.Security);
    }

    internal class IdentityUserTokenEntityMap : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder) =>
            builder.ToTable("UserTokens", SchemaNames.Security);
    }
}