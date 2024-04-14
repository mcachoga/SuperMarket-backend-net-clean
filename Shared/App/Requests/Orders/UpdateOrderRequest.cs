namespace SuperMarket.Shared.Requests.Catalog
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int MarketId { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }
    }
}