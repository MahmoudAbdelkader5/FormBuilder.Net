namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class DocuSignAccountInfoDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string BaseUri { get; set; } = string.Empty;
    }

    public class DocuSignSignerDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class DocuSignEnvelopeRequestDto
    {
        public int SubmissionId { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public DocuSignSignerDto Signer { get; set; } = new();
    }

    public class DocuSignRecipientViewRequestDto
    {
        public string EnvelopeId { get; set; } = string.Empty;
        public DocuSignSignerDto Signer { get; set; } = new();
        public string ReturnUrl { get; set; } = string.Empty;
    }
}

