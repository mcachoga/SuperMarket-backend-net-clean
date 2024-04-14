using SuperMarket.Domain.Abstractions;

namespace SuperMarket.Domain
{
    public class Order : AuditableEntity
    {
        public string UserId { get; set; }
        
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int MarketId { get; set; }
        public virtual Market Market { get; set; }

        public decimal Price { get; set; }
        
        public DateTime OrderDate { get; set; }
    }
}