using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Domain;

namespace SuperMarket.Infrastructure.Configuration.EntityMaps
{
    internal class ProductEntityMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .ToTable("Products", SchemaNames.Catalog)
                .HasIndex(e => e.Barcode)
                .HasDatabaseName("IX_Products_Barcode");

            builder
                .HasIndex(e => e.Name)
                .HasDatabaseName("IX_Products_Name");
        }
    }
}