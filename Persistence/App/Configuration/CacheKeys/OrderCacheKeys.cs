namespace SuperMarket.Persistence.Configuration.Cache
{
    internal static class OrderCacheKeys
    {
        public static string ListKey => "OrderList";

        public static string SelectListKey => "OrderSelectList";

        public static string GetKey(int orderId) => $"Order-{orderId}";

        public static string GetDetailsKey(int orderId) => $"OrderDetails-{orderId}";
    }
}