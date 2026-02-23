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

        var layoutCandidates = await GetLayoutCandidatesAsync(idLayout, idObject, cancellationToken);
        if (layoutCandidates.Count == 0)
            return NotFound("No active crystal layout found for this submission.");

        (byte[] Content, string ContentType, string? FileName) result = default;
        InvalidOperationException? lastLayoutNotFoundError = null;

        foreach (var candidateLayoutId in layoutCandidates)
        {
            try
            {
                result = await _crystalReportProxyService.GenerateLayoutPdfAsync(
                    candidateLayoutId,
                    idObject,
                    outputName,
                    printedByUserId,
                    cancellationToken);

                var downloadedFileName = string.IsNullOrWhiteSpace(result.FileName)
                    ? $"{outputName}.pdf"
                    : result.FileName;

                return File(result.Content, result.ContentType, downloadedFileName);
            }
            catch (InvalidOperationException ex) when (IsLayoutMissingOrInactiveError(ex))
            {
                lastLayoutNotFoundError = ex;
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        if (lastLayoutNotFoundError != null)
            return Problem(detail: lastLayoutNotFoundError.Message, statusCode: StatusCodes.Status500InternalServerError);

        return Problem(detail: "Failed to resolve an active crystal layout.", statusCode: StatusCodes.Status500InternalServerError);
    }

    private static bool IsLayoutMissingOrInactiveError(InvalidOperationException ex) =>
        ex.Message.Contains("Layout not found or inactive", StringComparison.OrdinalIgnoreCase);

    private async Task<List<int>> GetLayoutCandidatesAsync(int requestedLayoutId, int objectId, CancellationToken cancellationToken)
    {
        var candidates = new List<int>();
        var seen = new HashSet<int>();

        var documentTypeId = await _formBuilderDbContext.FORM_SUBMISSIONS
            .AsNoTracking()
            .Where(x => x.Id == objectId && !x.IsDeleted)
            .Select(x => (int?)x.DocumentTypeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!documentTypeId.HasValue || documentTypeId.Value <= 0)
            return candidates;

        if (requestedLayoutId > 0)
        {
            var requestedLayout = await _formBuilderDbContext.CRYSTAL_LAYOUTS
                .AsNoTracking()
                .Where(x => x.Id == requestedLayoutId && x.IsActive && !x.IsDeleted)
                .Select(x => new { x.Id, x.DocumentTypeId })
                .FirstOrDefaultAsync(cancellationToken);

            if (requestedLayout != null
                && requestedLayout.DocumentTypeId == documentTypeId.Value
                && seen.Add(requestedLayout.Id))
            {
                candidates.Add(requestedLayout.Id);
            }
        }

        var activeLayoutIds = await _formBuilderDbContext.CRYSTAL_LAYOUTS
            .AsNoTracking()
            .Where(x => x.DocumentTypeId == documentTypeId.Value && x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Id)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var layoutId in activeLayoutIds)
        {
            if (seen.Add(layoutId))
                candidates.Add(layoutId);
        }

        return candidates;
    }
}
