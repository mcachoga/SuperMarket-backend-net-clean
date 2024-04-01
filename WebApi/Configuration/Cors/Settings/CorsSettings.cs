namespace SuperMarket.WebApi.Configuration;

class CorsSettings
{
    public string PolicyName { get; init; } = null!;

    public string[] AllowedOrigins { get; init; } = null!;
}