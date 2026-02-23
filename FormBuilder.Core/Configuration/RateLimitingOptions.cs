namespace FormBuilder.Core.Configuration
{
    public class RateLimitingOptions
    {
        public const string SectionName = "RateLimiting";

        public bool Enabled { get; set; } = true;

        public RateLimitConfig GlobalLimit { get; set; } = new()
        {
            MaxRequests = 100,
            TimeWindowMinutes = 1
        };

        public Dictionary<string, RateLimitConfig> EndpointLimits { get; set; } = new();

        public List<string> Whitelist { get; set; } = new();

        public List<string> Blacklist { get; set; } = new();

        public List<string> BypassPaths { get; set; } = new()
        {
            "/swagger",
            "/health"
        };
    }

    public class RateLimitConfig
    {
        public int MaxRequests { get; set; } = 100;
        public int TimeWindowMinutes { get; set; } = 1;
    }
}
