using FormBuilder.Core.Configuration;
using FormBuilder.Core.IServices;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace FormBuilder.Services.Services.Email
{
    /// <summary>
    /// Email template service for processing templates with placeholders
    /// </summary>
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly EmailOptions _emailOptions;
        private readonly FormBuilderDbContext _dbContext;
        private readonly ILogger<EmailTemplateService> _logger;

        public EmailTemplateService(
            IOptions<EmailOptions> emailOptions,
            FormBuilderDbContext dbContext,
            ILogger<EmailTemplateService> logger)
        {
            _emailOptions = emailOptions.Value;
            _dbContext = dbContext;
            _logger = logger;
        }

        public string ProcessTemplate(string template, Dictionary<string, string> data)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return template;
            }

            var processedTemplate = template;

            // Handle simple conditional blocks:
            // {{#if Key}} ... {{/if}} will be included only if data[Key] is not null/empty.
            var ifPattern = @"\{\{#if\s+(\w+)\s*\}\}([\s\S]*?)\{\{\/if\}\}";
            processedTemplate = Regex.Replace(
                processedTemplate,
                ifPattern,
                match =>
                {
                    var key = match.Groups[1].Value;
                    if (data.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
                    {
                        return match.Groups[2].Value;
                    }
                    return string.Empty;
                },
                RegexOptions.IgnoreCase);

            // Replace placeholders like {{PlaceholderName}}
            var placeholderPattern = @"\{\{(\w+)\}\}";
            var matches = Regex.Matches(processedTemplate, placeholderPattern);

            foreach (Match match in matches)
            {
                var placeholderName = match.Groups[1].Value;
                if (data.TryGetValue(placeholderName, out var value))
                {
                    processedTemplate = processedTemplate.Replace(match.Value, value ?? string.Empty);
                }
                else
                {
                    _logger?.LogWarning("Placeholder '{PlaceholderName}' not found in template data", placeholderName);
                    processedTemplate = processedTemplate.Replace(match.Value, string.Empty);
                }
            }

            return processedTemplate;
        }

        public async Task<(string Subject, string Body)> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default)
        {
            // Try DB first (default template by code/name)
            var code = NormalizeCodeToTemplateCode(templateName);
            var dbTemplate = await _dbContext.Set<EMAIL_TEMPLATES>()
                .AsNoTracking()
                .Where(t => !t.IsDeleted && t.IsActive && t.IsDefault && t.TemplateCode.ToLower() == code.ToLower())
                .OrderByDescending(t => t.UpdatedDate ?? t.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbTemplate != null)
            {
                return (dbTemplate.SubjectTemplate ?? string.Empty, dbTemplate.BodyTemplateHtml ?? string.Empty);
            }

            return templateName.ToLower() switch
            {
                "submissionconfirmation" or "submission_confirmation" => 
                    (_emailOptions.Templates.SubmissionConfirmation.Subject, _emailOptions.Templates.SubmissionConfirmation.Body),
                
                "approvalrequired" or "approval_required" => 
                    (_emailOptions.Templates.ApprovalRequired.Subject, _emailOptions.Templates.ApprovalRequired.Body),
                
                "approvalresult" or "approval_result" => 
                    (_emailOptions.Templates.ApprovalResult.Subject, _emailOptions.Templates.ApprovalResult.Body),
                
                _ => throw new ArgumentException($"Template '{templateName}' not found", nameof(templateName))
            };
        }

        public async Task<(string Subject, string Body)> GetProcessedTemplateAsync(string templateName, Dictionary<string, string> data, CancellationToken cancellationToken = default)
        {
            var (subject, body) = await GetTemplateAsync(templateName, cancellationToken);
            
            // Add SystemUrl to data if not present
            if (!data.ContainsKey("SystemUrl"))
            {
                data["SystemUrl"] = _emailOptions.SystemUrl;
            }

            var processedSubject = ProcessTemplate(subject, data);
            var processedBody = ProcessTemplate(body, data);

            return (processedSubject, processedBody);
        }

        private static string NormalizeCodeToTemplateCode(string templateName)
        {
            var key = (templateName ?? string.Empty).Trim().ToLowerInvariant();
            return key switch
            {
                "submissionconfirmation" or "submission_confirmation" => "SubmissionConfirmation",
                "approvalrequired" or "approval_required" => "ApprovalRequired",
                "approvalresult" or "approval_result" => "ApprovalResult",
                _ => (templateName ?? string.Empty).Trim()
            };
        }
    }
}

