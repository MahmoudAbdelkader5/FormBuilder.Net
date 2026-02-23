namespace FormBuilder.Core.IServices
{
    /// <summary>
    /// Interface for email template processing
    /// </summary>
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Processes a template with placeholders
        /// </summary>
        /// <param name="template">Template string with placeholders like {{PlaceholderName}}</param>
        /// <param name="data">Dictionary of placeholder values</param>
        /// <returns>Processed template string</returns>
        string ProcessTemplate(string template, Dictionary<string, string> data);

        /// <summary>
        /// Gets a template by name
        /// </summary>
        Task<(string Subject, string Body)> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes and gets a template with data
        /// </summary>
        Task<(string Subject, string Body)> GetProcessedTemplateAsync(string templateName, Dictionary<string, string> data, CancellationToken cancellationToken = default);
    }
}

