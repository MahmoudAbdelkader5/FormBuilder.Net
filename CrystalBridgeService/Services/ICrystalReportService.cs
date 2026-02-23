using CrystalBridgeService.Models;

namespace CrystalBridgeService.Services
{
    public interface ICrystalReportService
    {
        ReportFileResult GenerateLayoutPdf(int idLayout, int idObject, string fileName, string printedByUserId);
    }
}
