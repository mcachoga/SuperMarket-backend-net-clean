using SuperMarket.Domain.Abstractions;

namespace SuperMarket.Domain
{
    public class Product : AuditableEntity
    {
        public string Barcode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}