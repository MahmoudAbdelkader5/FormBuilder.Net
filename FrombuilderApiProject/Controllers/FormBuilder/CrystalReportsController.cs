using FormBuilder.Core.IServices;
using FormBuilder.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FormBuilder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CrystalReportsController : ControllerBase
{
    private readonly ICrystalReportProxyService _crystalReportProxyService;
    private readonly FormBuilderDbContext _formBuilderDbContext;

    public CrystalReportsController(
        ICrystalReportProxyService crystalReportProxyService,
        FormBuilderDbContext formBuilderDbContext)
    {
        _crystalReportProxyService = crystalReportProxyService;
        _formBuilderDbContext = formBuilderDbContext;
    }

    [HttpGet("default-layout/{documentTypeId:int}")]
    public async Task<IActionResult> GetDefaultLayout(int documentTypeId, CancellationToken cancellationToken)
    {
        if (documentTypeId <= 0)
            return BadRequest("documentTypeId must be greater than zero.");

        var layout = await _formBuilderDbContext.CRYSTAL_LAYOUTS
            .AsNoTracking()
            .Where(x => x.DocumentTypeId == documentTypeId && x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.LayoutName)
            .Select(x => new
            {
                x.Id,
                x.LayoutName,
                x.IsDefault
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (layout == null)
            return NotFound("No active crystal layout found for this document type.");

        return Ok(layout);
    }

    [HttpGet("default-layouts")]
    public async Task<IActionResult> GetDefaultLayouts(
        [FromQuery] int[] documentTypeIds,
        CancellationToken cancellationToken)
    {
        if (documentTypeIds == null || documentTypeIds.Length == 0)
            return Ok(Array.Empty<object>());

        var normalizedIds = documentTypeIds.Where(x => x > 0).Distinct().ToArray();
        if (normalizedIds.Length == 0)
            return Ok(Array.Empty<object>());

        var layouts = await _formBuilderDbContext.CRYSTAL_LAYOUTS
            .AsNoTracking()
            .Where(x => normalizedIds.Contains(x.DocumentTypeId) && x.IsActive && !x.IsDeleted)
            .GroupBy(x => x.DocumentTypeId)
            .Select(g => g
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.LayoutName)
                .Select(x => new
                {
                    DocumentTypeId = x.DocumentTypeId,
                    x.Id,
                    x.LayoutName,
                    x.IsDefault
                })
                .First())
            .ToListAsync(cancellationToken);

        return Ok(layouts);
    }

    [HttpGet("layout/{idLayout:int}/object/{idObject:int}")]
    [Produces("application/pdf")]
    public async Task<IActionResult> GenerateLayoutPdf(
        int idLayout,
        int idObject,
        [FromQuery] string? fileName,
        CancellationToken cancellationToken)
    {
        if (idLayout <= 0 || idObject <= 0)
            return BadRequest("idLayout and idObject must be greater than zero.");

        var outputName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
        var printedByUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        (byte[] Content, string ContentType, string? FileName) result;
        try
        {
            result = await _crystalReportProxyService.GenerateLayoutPdfAsync(
                idLayout,
                idObject,
                outputName,
                printedByUserId,
                cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        var downloadedFileName = string.IsNullOrWhiteSpace(result.FileName)
            ? $"{outputName}.pdf"
            : result.FileName;

        return File(result.Content, result.ContentType, downloadedFileName);
    }
}
