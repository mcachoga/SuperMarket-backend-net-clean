using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Domain;

namespace SuperMarket.Persistence.Configuration.EntityMaps
{
    internal class OrderEntityMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .ToTable("Orders", SchemaNames.Catalog)
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Orders_UserId");
        }
    }
}