using formBuilder.Domian.Interfaces;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class DocumentNumberGeneratorService : IDocumentNumberGeneratorService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly ILogger<DocumentNumberGeneratorService>? _logger;

        public DocumentNumberGeneratorService(
            IunitOfwork unitOfWork,
            ILogger<DocumentNumberGeneratorService>? logger = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
        }

        public async Task<DocumentNumberGenerationResultDto> GenerateForSubmissionAsync(
            int submissionId,
            string generatedOn,
            string? generatedByUserId = null)
        {
            var context = (FormBuilderDbContext)_unitOfWork.AppDbContext;
            var database = context.Database;
            var ownsTransaction = database.CurrentTransaction == null;
            using var transaction = ownsTransaction
                ? await database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted)
                : null;

            try
            {
                var submission = await context.FORM_SUBMISSIONS
                    .FirstOrDefaultAsync(s => s.Id == submissionId && !s.IsDeleted);
                if (submission == null)
                {
                    return new DocumentNumberGenerationResultDto
                    {
                        Success = false,
                        ErrorMessage = "Submission not found."
                    };
                }

                var series = await context.DOCUMENT_SERIES
                    .FromSqlRaw("SELECT * FROM DOCUMENT_SERIES WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}", submission.SeriesId)
                    .FirstOrDefaultAsync();

                if (series == null || !series.IsActive || series.IsDeleted)
                {
                    return new DocumentNumberGenerationResultDto
                    {
                        Success = false,
                        ErrorMessage = "Document series not found or inactive."
                    };
                }

                if (!DocumentSeriesEngineRules.TryValidateTemplate(series.Template, out var templateError))
                {
                    return new DocumentNumberGenerationResultDto
                    {
                        Success = false,
                        ErrorMessage = templateError
                    };
                }

                if (!DocumentSeriesEngineRules.TryNormalizeResetPolicy(series.ResetPolicy, out var resetPolicy))
                {
                    return new DocumentNumberGenerationResultDto
                    {
                        Success = false,
                        ErrorMessage = $"Invalid reset policy '{series.ResetPolicy}'."
                    };
                }

                var utcNow = DateTime.UtcNow;
                var periodKey = DocumentSeriesEngineRules.BuildPeriodKey(resetPolicy, utcNow);
                var counters = context.Set<DOCUMENT_SERIES_COUNTERS>();

                var counter = await counters
                    .FromSqlRaw(
                        "SELECT * FROM DOCUMENT_SERIES_COUNTERS WITH (UPDLOCK, ROWLOCK) WHERE SeriesId = {0} AND PeriodKey = {1}",
                        series.Id,
                        periodKey)
                    .FirstOrDefaultAsync();

                int sequenceNumber;
                if (counter == null)
                {
                    sequenceNumber = series.SequenceStart > 0 ? series.SequenceStart : 1;
                    counter = new DOCUMENT_SERIES_COUNTERS
                    {
                        SeriesId = series.Id,
                        PeriodKey = periodKey,
                        CurrentNumber = sequenceNumber,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedDate = utcNow,
                        UpdatedDate = utcNow
                    };
                    counters.Add(counter);
                }
                else
                {
                    sequenceNumber = counter.CurrentNumber + 1;
                    counter.CurrentNumber = sequenceNumber;
                    counter.UpdatedDate = utcNow;
                    counters.Update(counter);
                }

                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(series.ProjectId);
                var projectCode = project?.Code ?? project?.Name ?? "NA";
                var documentNumber = DocumentSeriesEngineRules.RenderTemplate(series, projectCode, utcNow, sequenceNumber);

                if (documentNumber.Length > 50)
                {
                    return new DocumentNumberGenerationResultDto
                    {
                        Success = false,
                        ErrorMessage = "Generated document number exceeds 50 characters."
                    };
                }

                series.NextNumber = Math.Max(series.NextNumber, sequenceNumber + 1);
                series.UpdatedDate = utcNow;
                context.DOCUMENT_SERIES.Update(series);

                submission.DocumentNumber = documentNumber;
                submission.UpdatedDate = utcNow;
                context.FORM_SUBMISSIONS.Update(submission);

                context.DOCUMENT_NUMBER_AUDIT.Add(new DOCUMENT_NUMBER_AUDIT
                {
                    FormSubmissionId = submission.Id,
                    SeriesId = series.Id,
                    GeneratedNumber = documentNumber,
                    TemplateUsed = series.Template,
                    GeneratedAt = utcNow,
                    GeneratedOn = generatedOn,
                    GeneratedByUserId = generatedByUserId ?? submission.SubmittedByUserId ?? "system",
                    CreatedDate = utcNow,
                    UpdatedDate = utcNow,
                    IsActive = true,
                    IsDeleted = false
                });

                await context.SaveChangesAsync();

                if (ownsTransaction && transaction != null)
                    await transaction.CommitAsync();

                return new DocumentNumberGenerationResultDto
                {
                    Success = true,
                    DocumentNumber = documentNumber,
                    SequenceNumber = sequenceNumber,
                    PeriodKey = periodKey
                };
            }
            catch (Exception ex)
            {
                if (ownsTransaction && transaction != null)
                    await transaction.RollbackAsync();

                _logger?.LogError(ex, "Failed to generate document number for submission {SubmissionId}", submissionId);
                return new DocumentNumberGenerationResultDto
                {
                    Success = false,
                    ErrorMessage = $"Failed to generate document number: {ex.Message}"
                };
            }
        }
    }
}
