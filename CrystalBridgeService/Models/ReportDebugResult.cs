using System.Collections.Generic;

namespace CrystalBridgeService.Models
{
    public sealed class ReportDebugResult
    {
        public int RequestedLayoutId { get; set; }
        public int ObjectId { get; set; }
        public int? SubmissionDocumentTypeId { get; set; }
        public string SubmissionDocumentNumber { get; set; } = string.Empty;
        public int? SelectedLayoutId { get; set; }
        public int? SelectedLayoutDocumentTypeId { get; set; }
        public string SelectedLayoutName { get; set; } = string.Empty;
        public string SelectedLayoutPath { get; set; } = string.Empty;
        public string ResolvedReportPath { get; set; } = string.Empty;
        public bool ReportFileExists { get; set; }
        public List<string> MainParameters { get; set; } = new List<string>();
        public List<string> SubreportParameters { get; set; } = new List<string>();
        public Dictionary<string, string> MatchedParameters { get; set; } = new Dictionary<string, string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public int ExportedPdfBytes { get; set; }
    }
}
