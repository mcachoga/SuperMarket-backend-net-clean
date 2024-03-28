using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Domain;

namespace SuperMarket.Infrastructure.Configuration.EntityMaps
{
    internal class AuditEntityMap : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder
                .ToTable("Audits", SchemaNames.Security)
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Orders_UserId");
        }
    }
}