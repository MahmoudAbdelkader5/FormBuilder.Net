using FormBuilder.Core.Configuration;
using FormBuilder.Core.IServices;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FormBuilder.Services.Services.Email
{
    /// <summary>
    /// Email service implementation using SMTP
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;
        private readonly ILogger<EmailService> _logger;
        private readonly FormBuilderDbContext _dbContext;
        private readonly ISecretProtector _protector;
        private readonly IMemoryCache _cache;

        public EmailService(
            IOptions<SmtpOptions> smtpOptions,
            FormBuilderDbContext dbContext,
            ISecretProtector protector,
            IMemoryCache cache,
            ILogger<EmailService> logger)
        {
            _smtpOptions = smtpOptions.Value;
            _dbContext = dbContext;
            _protector = protector;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            return await SendEmailAsync(new[] { to }, subject, body, isHtml, cancellationToken);
        }

        public async Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            // Default behavior: use active SMTP config from DB if available, otherwise fallback to appsettings.
            return await SendEmailInternalAsync(to, subject, body, smtpConfigId: null, isHtml, cancellationToken);
        }

        public async Task<bool> SendEmailWithRetryAsync(string to, string subject, string body, bool isHtml = true, int? retryAttempts = null, CancellationToken cancellationToken = default)
        {
            var attempts = retryAttempts ?? _smtpOptions.RetryAttempts;
            var delay = TimeSpan.FromSeconds(_smtpOptions.RetryDelaySeconds);

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    var result = await SendEmailAsync(to, subject, body, isHtml, cancellationToken);
                    if (result)
                    {
                        return true;
                    }

                    if (i < attempts - 1)
                    {
                        _logger?.LogWarning("Email send failed, retrying in {Delay} seconds. Attempt {Attempt}/{Total}", 
                            delay.TotalSeconds, i + 1, attempts);
                        await Task.Delay(delay, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Exception during email send attempt {Attempt}/{Total}", i + 1, attempts);
                    if (i < attempts - 1)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }
            }

            _logger?.LogError("Failed to send email after {Attempts} attempts. To: {To}, Subject: {Subject}", 
                attempts, to, subject);
            return false;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body, int smtpConfigId, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            return SendEmailAsync(new[] { to }, subject, body, smtpConfigId, isHtml, cancellationToken);
        }

        public Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string body, int smtpConfigId, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            return SendEmailInternalAsync(to, subject, body, smtpConfigId, isHtml, cancellationToken);
        }

        public async Task<bool> SendEmailWithRetryAsync(string to, string subject, string body, int smtpConfigId, bool isHtml = true, int? retryAttempts = null, CancellationToken cancellationToken = default)
        {
            var attempts = retryAttempts ?? _smtpOptions.RetryAttempts;
            var delay = TimeSpan.FromSeconds(_smtpOptions.RetryDelaySeconds);

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    var result = await SendEmailInternalAsync(new[] { to }, subject, body, smtpConfigId, isHtml, cancellationToken);
                    if (result) return true;

                    if (i < attempts - 1)
                    {
                        _logger?.LogWarning("Email send failed, retrying in {Delay} seconds. Attempt {Attempt}/{Total}",
                            delay.TotalSeconds, i + 1, attempts);
                        await Task.Delay(delay, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Exception during email send attempt {Attempt}/{Total}", i + 1, attempts);
                    if (i < attempts - 1)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }
            }

            _logger?.LogError("Failed to send email after {Attempts} attempts. To: {To}, Subject: {Subject}",
                attempts, to, subject);
            return false;
        }

        private async Task<bool> SendEmailInternalAsync(IEnumerable<string> to, string subject, string body, int? smtpConfigId, bool isHtml, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.LogInformation("Starting SendEmailInternalAsync. Recipients: {Recipients}, Subject: {Subject}, SmtpConfigId: {SmtpConfigId}, IsHtml: {IsHtml}",
                    string.Join(", ", to), subject, smtpConfigId?.ToString() ?? "null", isHtml);

                var recipients = to.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                if (!recipients.Any())
                {
                    _logger?.LogWarning("No valid recipients for email. Subject: {Subject}, Original recipients: {Recipients}", 
                        subject, string.Join(", ", to));
                    return false;
                }

                _logger?.LogInformation("Resolving SMTP configuration. SmtpConfigId: {SmtpConfigId}", smtpConfigId?.ToString() ?? "null");
                var smtp = await ResolveSmtpAsync(smtpConfigId, cancellationToken);
                if (smtp == null)
                {
                    _logger?.LogError("No SMTP configuration found (DB or appsettings). Email will not be sent. " +
                        "Subject: {Subject}, SmtpConfigId: {SmtpConfigId}. " +
                        "Please check: 1) SMTP_CONFIGS table has active record, or 2) appsettings.json has Smtp:Host configured.",
                        subject, smtpConfigId?.ToString() ?? "null");
                    return false;
                }

                _logger?.LogInformation("SMTP configuration resolved. Host: {Host}, Port: {Port}, FromEmail: {FromEmail}, FromName: {FromName}, UseSsl: {UseSsl}",
                    smtp.Host, smtp.Port, smtp.FromEmail, smtp.FromName, smtp.UseSsl);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(smtp.FromName, smtp.FromEmail));

                foreach (var recipient in recipients)
                {
                    message.To.Add(MailboxAddress.Parse(recipient));
                }

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml) bodyBuilder.HtmlBody = body;
                else bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                _logger?.LogInformation("Connecting to SMTP server {Host}:{Port} with SSL option: {SecureSocketOption}",
                    smtp.Host, smtp.Port, GetSecureSocketOptions(smtp));

                using var client = new SmtpClient();
                await client.ConnectAsync(smtp.Host, smtp.Port, GetSecureSocketOptions(smtp), cancellationToken);
                _logger?.LogInformation("Connected to SMTP server successfully");

                if (!string.IsNullOrWhiteSpace(smtp.Username) && !string.IsNullOrWhiteSpace(smtp.Password))
                {
                    _logger?.LogInformation("Authenticating with SMTP server using username: {Username}", smtp.Username);
                    await client.AuthenticateAsync(smtp.Username, smtp.Password, cancellationToken);
                    _logger?.LogInformation("SMTP authentication successful");
                }
                else
                {
                    _logger?.LogInformation("No SMTP credentials provided, skipping authentication");
                }

                _logger?.LogInformation("Sending email to {Count} recipients: {Recipients}", recipients.Count, string.Join(", ", recipients));
                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                _logger?.LogInformation("Email sent successfully to {Recipients}. Subject: {Subject}",
                    string.Join(", ", recipients), subject);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send email to {Recipients}. Subject: {Subject}. Error: {ErrorMessage}. StackTrace: {StackTrace}",
                    string.Join(", ", to), subject, ex.Message, ex.StackTrace);
                return false;
            }
        }

        private sealed class ResolvedSmtp
        {
            public required string Host { get; init; }
            public required int Port { get; init; }
            public required bool UseSsl { get; init; }
            public required string Username { get; init; }
            public required string Password { get; init; }
            public required string FromEmail { get; init; }
            public required string FromName { get; init; }
        }

        private async Task<ResolvedSmtp?> ResolveSmtpAsync(int? smtpConfigId, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Resolving SMTP configuration. SmtpConfigId: {SmtpConfigId}", smtpConfigId?.ToString() ?? "null");

            // Try DB first
            var cacheKey = smtpConfigId.HasValue ? $"smtp:{smtpConfigId.Value}" : "smtp:active";
            if (_cache.TryGetValue(cacheKey, out ResolvedSmtp cached))
            {
                _logger?.LogInformation("SMTP configuration found in cache. Host: {Host}, Port: {Port}", cached.Host, cached.Port);
                return cached;
            }

            SMTP_CONFIGS? dbConfig = null;
            if (smtpConfigId.HasValue)
            {
                _logger?.LogInformation("Looking for SMTP config in database with Id: {SmtpConfigId}", smtpConfigId.Value);
                dbConfig = await _dbContext.Set<SMTP_CONFIGS>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == smtpConfigId.Value && !x.IsDeleted && x.IsActive, cancellationToken);
                
                if (dbConfig != null)
                {
                    _logger?.LogInformation("Found SMTP config in database. Id: {Id}, Host: {Host}, Port: {Port}, IsActive: {IsActive}, IsDeleted: {IsDeleted}",
                        dbConfig.Id, dbConfig.Host, dbConfig.Port, dbConfig.IsActive, dbConfig.IsDeleted);
                }
                else
                {
                    _logger?.LogWarning("SMTP config not found in database with Id: {SmtpConfigId} (or not active/deleted)", smtpConfigId.Value);
                }
            }
            else
            {
                _logger?.LogInformation("Looking for active SMTP config in database (no specific ID)");
                dbConfig = await _dbContext.Set<SMTP_CONFIGS>()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted && x.IsActive)
                    .OrderByDescending(x => x.UpdatedDate ?? x.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (dbConfig != null)
                {
                    _logger?.LogInformation("Found active SMTP config in database. Id: {Id}, Host: {Host}, Port: {Port}",
                        dbConfig.Id, dbConfig.Host, dbConfig.Port);
                }
                else
                {
                    _logger?.LogWarning("No active SMTP config found in database. Checking appsettings fallback...");
                }
            }

            ResolvedSmtp? resolved = null;

            if (dbConfig != null)
            {
                try
                {
                    var password = _protector.Unprotect(dbConfig.PasswordEncrypted);
                    resolved = new ResolvedSmtp
                    {
                        Host = dbConfig.Host,
                        Port = dbConfig.Port,
                        UseSsl = dbConfig.UseSsl,
                        Username = dbConfig.UserName,
                        Password = password,
                        FromEmail = dbConfig.FromEmail,
                        FromName = dbConfig.FromDisplayName
                    };
                    _logger?.LogInformation("SMTP config resolved from database. Host: {Host}, Port: {Port}, FromEmail: {FromEmail}",
                        resolved.Host, resolved.Port, resolved.FromEmail);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to decrypt SMTP password for config Id: {Id}. Error: {ErrorMessage}",
                        dbConfig.Id, ex.Message);
                }
            }
            else if (!string.IsNullOrWhiteSpace(_smtpOptions.Host))
            {
                // Fallback to appsettings
                _logger?.LogInformation("Using SMTP config from appsettings. Host: {Host}, Port: {Port}",
                    _smtpOptions.Host, _smtpOptions.Port);
                resolved = new ResolvedSmtp
                {
                    Host = _smtpOptions.Host,
                    Port = _smtpOptions.Port,
                    UseSsl = _smtpOptions.EnableSsl,
                    Username = _smtpOptions.Username,
                    Password = _smtpOptions.Password,
                    FromEmail = _smtpOptions.FromEmail,
                    FromName = _smtpOptions.FromName
                };
            }
            else
            {
                _logger?.LogError("No SMTP configuration found in database or appsettings. " +
                    "Please configure SMTP_CONFIGS table with IsActive=true or set Smtp:Host in appsettings.json");
            }

            if (resolved != null)
            {
                _cache.Set(cacheKey, resolved, TimeSpan.FromMinutes(5));
                _logger?.LogInformation("SMTP configuration cached for 5 minutes");
            }

            return resolved;
        }

        private SecureSocketOptions GetSecureSocketOptions(ResolvedSmtp smtp)
        {
            // Port 465 requires implicit SSL (SSL from the start)
            // Port 587 uses STARTTLS (explicit TLS)
            if (smtp.Port == 465)
            {
                // For port 465, use implicit SSL
                return SecureSocketOptions.SslOnConnect;
            }
            else if (smtp.Port == 587)
            {
                // For port 587, use STARTTLS
                return SecureSocketOptions.StartTls;
            }
            else
            {
                // For other ports, determine based on UseSsl flag (DB) or EnableSsl (config)
                return smtp.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
            }
        }
    }
}

