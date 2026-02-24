using FormBuilder.Core.IServices;
using FormBuilder.Services.Services.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FormBuilder.API.Controllers
{
    /// <summary>
    /// Controller for testing email functionality
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly EmailNotificationService _emailNotificationService;
        private readonly ILogger<EmailTestController> _logger;

        public EmailTestController(
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            EmailNotificationService emailNotificationService,
            ILogger<EmailTestController> logger)
        {
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        /// <summary>
        /// Test sending a simple email
        /// </summary>
        [HttpPost("send-simple")]
        public async Task<IActionResult> SendSimpleEmail([FromBody] SendTestEmailDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.To))
                {
                    return BadRequest(new { statusCode = 400, message = "Email address is required" });
                }

                var result = await _emailService.SendEmailAsync(
                    dto.To,
                    dto.Subject ?? "Test Email",
                    dto.Body ?? "This is a test email from Form Builder System.",
                    isHtml: dto.IsHtml ?? true);

                if (result)
                {
                    return Ok(new { statusCode = 200, message = "Email sent successfully", to = dto.To });
                }
                else
                {
                    return StatusCode(500, new { statusCode = 500, message = "Failed to send email" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                return StatusCode(500, new { statusCode = 500, message = "Error sending email", error = ex.Message });
            }
        }

        /// <summary>
        /// Test email template processing
        /// </summary>
        [HttpPost("test-template")]
        public async Task<IActionResult> TestTemplate([FromBody] TestTemplateDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.TemplateName))
                {
                    return BadRequest(new { statusCode = 400, message = "Template name is required" });
                }

                // Convert Dictionary<string, object> to Dictionary<string, string>
                var templateData = new Dictionary<string, string>();
                if (dto.Data != null)
                {
                    foreach (var kvp in dto.Data)
                    {
                        templateData[kvp.Key] = kvp.Value?.ToString() ?? string.Empty;
                    }
                }

                var (subject, body) = await _emailTemplateService.GetProcessedTemplateAsync(
                    dto.TemplateName,
                    templateData,
                    CancellationToken.None);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Template processed successfully",
                    templateName = dto.TemplateName,
                    subject,
                    body
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing template");
                return StatusCode(500, new { statusCode = 500, message = "Error processing template", error = ex.Message });
            }
        }

        /// <summary>
        /// Test sending submission confirmation email
        /// </summary>
        [HttpPost("test-submission-confirmation/{submissionId}")]
        public async Task<IActionResult> TestSubmissionConfirmation(int submissionId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _emailNotificationService.SendSubmissionConfirmationAsync(submissionId);
                return Ok(new { statusCode = 200, message = "Submission confirmation email sent", submissionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending submission confirmation email");
                return StatusCode(500, new { statusCode = 500, message = "Error sending email", error = ex.Message });
            }
        }

        /// <summary>
        /// Test sending approval required email
        /// </summary>
        [HttpPost("test-approval-required")]
        public async Task<IActionResult> TestApprovalRequired([FromBody] TestApprovalRequiredDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (dto.SubmissionId <= 0 || dto.StageId <= 0)
                {
                    return BadRequest(new { statusCode = 400, message = "SubmissionId and StageId are required" });
                }

                if (dto.ApproverUserIds == null || !dto.ApproverUserIds.Any())
                {
                    return BadRequest(new { statusCode = 400, message = "At least one approver user ID is required" });
                }

                await _emailNotificationService.SendApprovalRequiredAsync(
                    dto.SubmissionId,
                    dto.StageId,
                    dto.ApproverUserIds);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Approval required email sent",
                    submissionId = dto.SubmissionId,
                    stageId = dto.StageId,
                    approverCount = dto.ApproverUserIds.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending approval required email");
                return StatusCode(500, new { statusCode = 500, message = "Error sending email", error = ex.Message });
            }
        }

        /// <summary>
        /// Test sending approval result email
        /// </summary>
        [HttpPost("test-approval-result")]
        public async Task<IActionResult> TestApprovalResult([FromBody] TestApprovalResultDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (dto.SubmissionId <= 0)
                {
                    return BadRequest(new { statusCode = 400, message = "SubmissionId is required" });
                }

                if (string.IsNullOrWhiteSpace(dto.ActionType))
                {
                    return BadRequest(new { statusCode = 400, message = "ActionType is required" });
                }

                if (string.IsNullOrWhiteSpace(dto.ApproverUserId))
                {
                    return BadRequest(new { statusCode = 400, message = "ApproverUserId is required" });
                }

                await _emailNotificationService.SendApprovalResultAsync(
                    dto.SubmissionId,
                    dto.ActionType,
                    dto.ApproverUserId,
                    dto.Comments);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Approval result email sent",
                    submissionId = dto.SubmissionId,
                    actionType = dto.ActionType,
                    approverUserId = dto.ApproverUserId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending approval result email");
                return StatusCode(500, new { statusCode = 500, message = "Error sending email", error = ex.Message });
            }
        }

        /// <summary>
        /// Get available email templates
        /// </summary>
        [HttpGet("templates")]
        public IActionResult GetTemplates()
        {
            var templates = new[]
            {
                new { name = "SubmissionConfirmation", description = "Sent when a form is submitted" },
                new { name = "ApprovalRequired", description = "Sent when approval is required" },
                new { name = "ApprovalResult", description = "Sent when a document is approved/rejected/returned" }
            };

            return Ok(new { statusCode = 200, message = "Available templates", templates });
        }
    }

    // DTOs
    public class SendTestEmailDto
    {
        public string To { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool? IsHtml { get; set; }
    }

    public class TestTemplateDto
    {
        public string TemplateName { get; set; } = string.Empty;
        public Dictionary<string, object>? Data { get; set; }
    }

    public class TestApprovalRequiredDto
    {
        public int SubmissionId { get; set; }
        public int StageId { get; set; }
        public IEnumerable<string> ApproverUserIds { get; set; } = Array.Empty<string>();
    }

    public class TestApprovalResultDto
    {
        public int SubmissionId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string ApproverUserId { get; set; } = string.Empty;
        public string? Comments { get; set; }
    }
}

