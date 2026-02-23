namespace FormBuilder.Core.IServices
{
    /// <summary>
    /// Interface for email sending operations
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (HTML or plain text)</param>
        /// <param name="isHtml">Whether the body is HTML</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email was sent successfully, false otherwise</returns>
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email to multiple recipients
        /// </summary>
        Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email with retry logic
        /// </summary>
        Task<bool> SendEmailWithRetryAsync(string to, string subject, string body, bool isHtml = true, int? retryAttempts = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email using a specific SMTP config from the database.
        /// </summary>
        Task<bool> SendEmailAsync(string to, string subject, string body, int smtpConfigId, bool isHtml = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email to multiple recipients using a specific SMTP config from the database.
        /// </summary>
        Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string body, int smtpConfigId, bool isHtml = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email with retry logic using a specific SMTP config from the database.
        /// </summary>
        Task<bool> SendEmailWithRetryAsync(string to, string subject, string body, int smtpConfigId, bool isHtml = true, int? retryAttempts = null, CancellationToken cancellationToken = default);
    }
}

