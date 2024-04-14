using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Domain;

namespace SuperMarket.Persistence.Configuration.EntityMaps
{
    internal class MarketEntityMap : IEntityTypeConfiguration<Market>
    {
        public void Configure(EntityTypeBuilder<Market> builder)
        {
            builder
                .ToTable("Markets", SchemaNames.Catalog)
                .HasIndex(e => e.Name)
                .HasDatabaseName("IX_Markets_Name");
        }
    }
}