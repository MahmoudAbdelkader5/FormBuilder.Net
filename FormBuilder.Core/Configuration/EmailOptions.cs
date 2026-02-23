namespace FormBuilder.Core.Configuration
{
    /// <summary>
    /// Email configuration options
    /// </summary>
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public string SystemUrl { get; set; } = "http://localhost:5203";
        
        public EmailTemplateOptions Templates { get; set; } = new();
    }

    public class EmailTemplateOptions
    {
        public EmailTemplate SubmissionConfirmation { get; set; } = new();
        public EmailTemplate ApprovalRequired { get; set; } = new();
        public EmailTemplate ApprovalResult { get; set; } = new();
    }

    public class EmailTemplate
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}

