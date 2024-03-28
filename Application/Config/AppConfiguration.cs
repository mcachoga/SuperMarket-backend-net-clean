namespace SuperMarket.Application.Configuration
{
    public class AppConfiguration
    {
        public string Secret { get; set; }

        public int TokenExpiryInMinutes { get; set; }
    }
}