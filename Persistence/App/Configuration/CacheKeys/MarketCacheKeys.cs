namespace SuperMarket.Persistence.Configuration.Cache
{
    internal static class MarketCacheKeys
    {
        public static string ListKey => "MarketList";

        public static string SelectListKey => "MarketSelectList";

        public static string GetKey(int marketId) => $"Market-{marketId}";

        public static string GetDetailsKey(int marketId) => $"MarketDetails-{marketId}";
    }
}