namespace SuperMarket.Domain.Auth
{
    public class TokenAuth 
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}