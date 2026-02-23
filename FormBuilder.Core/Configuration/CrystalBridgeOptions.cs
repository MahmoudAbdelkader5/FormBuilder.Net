namespace FormBuilder.Core.Configuration;

public sealed class CrystalBridgeOptions
{
    public const string SectionName = "CrystalBridge";

    public string BaseUrl { get; set; } = string.Empty;
    public string GenerateLayoutPath { get; set; } = "api/reports/GenerateLayout";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 120;
}
