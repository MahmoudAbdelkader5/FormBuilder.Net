namespace FormBuilder.Core.IServices;

public interface ICrystalReportProxyService
{
    Task<(byte[] Content, string ContentType, string? FileName)> GenerateLayoutPdfAsync(
        int idLayout,
        int idObject,
        string fileName,
        string? printedByUserId = null,
        CancellationToken cancellationToken = default);
}
