using CrystalBridge.Models;

namespace CrystalBridge.Services
{
    public interface ICrystalReportService
    {
        ReportFileResult GenerateLayoutPdf(int idLayout, int idObject, string fileName, string printedByUserId);
        ReportDebugResult GenerateLayoutDebug(int idLayout, int idObject, string fileName, string printedByUserId);
    }
}
