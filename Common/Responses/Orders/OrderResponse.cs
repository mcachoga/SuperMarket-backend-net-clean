namespace SuperMarket.Common.Responses.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int MarketId { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }
    }
}