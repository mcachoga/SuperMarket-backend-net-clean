namespace SuperMarket.Common.Requests.Orders
{
    public class CreateOrderRequest
    {
        public int ProductId { get; set; }

        public int MarketId { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }
    }
}