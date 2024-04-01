using System.ComponentModel.DataAnnotations;

namespace SuperMarket.WebApi.Configuration;

class SwaggerSettings
{
    [Required, MinLength(1)]
    public string ApiName { get; init; } = null!;

    public string ApiVersion { get; init; }

    public bool UseSwagger { get; init; }

    public string LoginPath { get; set; } = null!;
}