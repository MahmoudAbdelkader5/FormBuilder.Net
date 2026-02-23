namespace CrystalBridge.Models
{
    public sealed class ReportFileResult
    {
        public byte[] Content { get; set; } = new byte[0];
        public string ContentType { get; set; } = "application/pdf";
        public string FileName { get; set; } = "Report.pdf";
    }
}

