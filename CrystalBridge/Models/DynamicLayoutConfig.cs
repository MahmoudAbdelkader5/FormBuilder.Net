namespace CrystalBridge.Models
{
    public sealed class DynamicLayoutConfig
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
    }
}

