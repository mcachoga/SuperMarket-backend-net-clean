namespace SuperMarket.Infrastructure.Extensions.Logging
{
    internal class SerilogSettings
    {
        public string WriteDefaultDev { get; init; } = string.Empty;

        public string WriteDefaultPro { get; init; } = string.Empty;

        public string ConsoleTemplate { get; init; } = "{Timestamp} [{Level}] {Message}{NewLine:1}";

        public string FilePathFormat { get; init; } = "Log\\log-{Date}.log";

        public string FileTemplate { get; init; } = "{Timestamp} [{Level}] {Message}{NewLine:1}";
    }
}