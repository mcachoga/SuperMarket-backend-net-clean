namespace SuperMarket.Persistence.Identity.Configuration
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public int TokenExpiryInMinutes { get; set; }

        public int TokenRefreshExpiryInDays { get; set; }
    }
}