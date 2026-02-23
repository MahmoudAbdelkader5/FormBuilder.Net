namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class SubmitSignatureFlowInputDto
    {
        public bool SignatureRequired { get; set; }
        public int SubmissionId { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string? ExistingEnvelopeId { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
        public DocuSignSignerDto Signer { get; set; } = new();
    }

    public class SubmitSignatureFlowResultDto
    {
        public bool SignatureRequired { get; set; }
        public string SignatureStatus { get; set; } = "not_required";
        public string? SigningUrl { get; set; }
        public string? EnvelopeId { get; set; }
        public bool DocuSignCalled { get; set; }
    }
}

