using System.Text;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class DocuSignEnvelopeService : IDocuSignEnvelopeService
    {
        private readonly IDocuSignAuthService _authService;

        public DocuSignEnvelopeService(IDocuSignAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> CreateEnvelopeAsync(DocuSignEnvelopeRequestDto request)
        {
            ValidateEnvelopeRequest(request);

            var accessToken = await _authService.GetAccessTokenAsync();
            var account = await _authService.GetAccountInfoAsync(accessToken);
            var apiClient = CreateBaseClient(accessToken, account.BaseUri);
            var envelopesApi = new EnvelopesApi(apiClient);

            var envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = $"Signature required for submission {request.DocumentNumber}",
                Status = "sent",
                Documents = new List<Document>
                {
                    new()
                    {
                        DocumentBase64 = Convert.ToBase64String(BuildDocumentBytes(request)),
                        Name = $"Submission-{request.DocumentNumber}.txt",
                        FileExtension = "txt",
                        DocumentId = "1"
                    }
                },
                Recipients = new Recipients
                {
                    Signers = new List<Signer>
                    {
                        new()
                        {
                            Email = request.Signer.Email,
                            Name = request.Signer.Name,
                            RecipientId = "1",
                            RoutingOrder = "1",
                            ClientUserId = request.Signer.UserId,
                            Tabs = new Tabs
                            {
                                SignHereTabs = new List<SignHere>
                                {
                                    new()
                                    {
                                        AnchorString = "/sig1/",
                                        AnchorUnits = "pixels",
                                        AnchorXOffset = "0",
                                        AnchorYOffset = "0"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var envelopeSummary = await envelopesApi.CreateEnvelopeAsync(account.AccountId, envelopeDefinition);
            if (string.IsNullOrWhiteSpace(envelopeSummary?.EnvelopeId))
                throw new InvalidOperationException("DocuSign did not return an envelope ID.");

            return envelopeSummary.EnvelopeId;
        }

        public async Task<string> CreateRecipientViewAsync(DocuSignRecipientViewRequestDto request)
        {
            ValidateRecipientViewRequest(request);

            var accessToken = await _authService.GetAccessTokenAsync();
            var account = await _authService.GetAccountInfoAsync(accessToken);
            var apiClient = CreateBaseClient(accessToken, account.BaseUri);
            var envelopesApi = new EnvelopesApi(apiClient);

            var viewRequest = new RecipientViewRequest
            {
                ReturnUrl = request.ReturnUrl,
                AuthenticationMethod = "none",
                UserName = request.Signer.Name,
                Email = request.Signer.Email,
                ClientUserId = request.Signer.UserId
            };

            var view = await envelopesApi.CreateRecipientViewAsync(account.AccountId, request.EnvelopeId, viewRequest);
            if (string.IsNullOrWhiteSpace(view?.Url))
                throw new InvalidOperationException("DocuSign did not return a recipient signing URL.");

            return view.Url;
        }

        private static DocuSignClient CreateBaseClient(string accessToken, string baseUri)
        {
            var normalizedAccessToken = accessToken?.Trim() ?? string.Empty;
            const string bearerPrefix = "Bearer ";
            if (normalizedAccessToken.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                normalizedAccessToken = normalizedAccessToken[bearerPrefix.Length..].Trim();

            var apiClient = new DocuSignClient($"{baseUri}/restapi");
            apiClient.Configuration.DefaultHeader["Authorization"] = $"Bearer {normalizedAccessToken}";
            return apiClient;
        }

        private static byte[] BuildDocumentBytes(DocuSignEnvelopeRequestDto request)
        {
            var content = $"""
Submission ID: {request.SubmissionId}
Document Number: {request.DocumentNumber}
Signer: {request.Signer.Name} <{request.Signer.Email}>

Please sign below:
/sig1/
""";
            return Encoding.UTF8.GetBytes(content);
        }

        private static void ValidateEnvelopeRequest(DocuSignEnvelopeRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.SubmissionId <= 0)
                throw new ArgumentException("SubmissionId is required.", nameof(request.SubmissionId));
            if (string.IsNullOrWhiteSpace(request.Signer?.Email))
                throw new ArgumentException("Signer email is required.", nameof(request.Signer.Email));
            if (string.IsNullOrWhiteSpace(request.Signer?.Name))
                throw new ArgumentException("Signer name is required.", nameof(request.Signer.Name));
            if (string.IsNullOrWhiteSpace(request.Signer?.UserId))
                throw new ArgumentException("Signer user ID is required for embedded signing.", nameof(request.Signer.UserId));
        }

        private static void ValidateRecipientViewRequest(DocuSignRecipientViewRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.EnvelopeId))
                throw new ArgumentException("EnvelopeId is required.", nameof(request.EnvelopeId));
            if (string.IsNullOrWhiteSpace(request.ReturnUrl))
                throw new ArgumentException("ReturnUrl is required.", nameof(request.ReturnUrl));
            if (string.IsNullOrWhiteSpace(request.Signer?.Email))
                throw new ArgumentException("Signer email is required.", nameof(request.Signer.Email));
            if (string.IsNullOrWhiteSpace(request.Signer?.Name))
                throw new ArgumentException("Signer name is required.", nameof(request.Signer.Name));
            if (string.IsNullOrWhiteSpace(request.Signer?.UserId))
                throw new ArgumentException("Signer user ID is required for embedded signing.", nameof(request.Signer.UserId));
        }
    }
}
