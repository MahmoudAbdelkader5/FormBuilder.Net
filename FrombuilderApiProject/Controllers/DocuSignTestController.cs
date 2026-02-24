using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/docusign-test")]
    public class DocuSignTestController : ControllerBase
    {
        private readonly IDocuSignAuthService _docuSignAuthService;
        private readonly ILogger<DocuSignTestController> _logger;

        public DocuSignTestController(
            IDocuSignAuthService docuSignAuthService,
            ILogger<DocuSignTestController> logger)
        {
            _docuSignAuthService = docuSignAuthService;
            _logger = logger;
        }

        [HttpGet("jwt")]
        public async Task<IActionResult> TestJwtAsync([FromQuery] string? expectedAccountId = null,
            [FromQuery] string? expectedBaseUri = null, CancellationToken cancellationToken = default)
        {
            var authServer = Environment.GetEnvironmentVariable("DS_AUTH_SERVER");
            var integrationKey = Environment.GetEnvironmentVariable("DS_INTEGRATION_KEY");
            var userId = Environment.GetEnvironmentVariable("DS_USER_ID");
            var privateKeyPath = Environment.GetEnvironmentVariable("DS_PRIVATE_KEY_PATH");

            var envCheck = new
            {
                dsAuthServer = authServer,
                dsIntegrationKeyMasked = MaskGuidLikeValue(integrationKey),
                dsUserId = userId,
                dsPrivateKeyPath = privateKeyPath,
                privateKeyFileExists = !string.IsNullOrWhiteSpace(privateKeyPath) && System.IO.File.Exists(privateKeyPath)
            };

            try
            {
                var token = await _docuSignAuthService.GetAccessTokenAsync();
                var account = await _docuSignAuthService.GetAccountInfoAsync(token);
                var rawAccounts = await GetUserInfoAccountsAsync(token, authServer);

                var accountIdMatches = string.IsNullOrWhiteSpace(expectedAccountId) ||
                                       string.Equals(account.AccountId, expectedAccountId, StringComparison.OrdinalIgnoreCase);

                var normalizedActualBaseUri = NormalizeBaseUri(account.BaseUri);
                var normalizedExpectedBaseUri = NormalizeBaseUri(expectedBaseUri);
                var baseUriMatches = string.IsNullOrWhiteSpace(expectedBaseUri) ||
                                     string.Equals(normalizedActualBaseUri, normalizedExpectedBaseUri, StringComparison.OrdinalIgnoreCase);

                return Ok(new
                {
                    ok = true,
                    jwt = new
                    {
                        accessTokenReceived = !string.IsNullOrWhiteSpace(token),
                        accessTokenLength = token.Length
                    },
                    account = new
                    {
                        accountId = account.AccountId,
                        baseUri = account.BaseUri
                    },
                    userInfoAccounts = rawAccounts.Select(a => new
                    {
                        accountIdMasked = MaskGuidLikeValue(a.AccountId),
                        baseUri = a.BaseUri,
                        isDefault = a.IsDefault,
                        matchesSelectedAccount = string.Equals(a.AccountId, account.AccountId, StringComparison.OrdinalIgnoreCase)
                    }),
                    expected = new
                    {
                        expectedAccountId,
                        expectedBaseUri,
                        accountIdMatches,
                        baseUriMatches
                    },
                    environment = envCheck
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DocuSign JWT test endpoint failed.");

                return StatusCode(500, new
                {
                    ok = false,
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    environment = envCheck
                });
            }
        }

        private static string? NormalizeBaseUri(string? uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return uri;

            return uri.Trim().TrimEnd('/').ToLowerInvariant();
        }

        private static string MaskGuidLikeValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var clean = value.Trim();
            if (clean.Length <= 8)
                return "********";

            return $"{clean[..4]}****{clean[^4..]}";
        }

        private static async Task<List<UserInfoAccount>> GetUserInfoAccountsAsync(string accessToken, string? authServer)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return new List<UserInfoAccount>();

            var normalizedToken = accessToken.Trim();
            const string bearerPrefix = "Bearer ";
            if (normalizedToken.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                normalizedToken = normalizedToken[bearerPrefix.Length..].Trim();

            var normalizedAuthServer = NormalizeAuthServer(authServer);
            var endpoint = $"https://{normalizedAuthServer}/oauth/userinfo";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", normalizedToken);

            var response = await httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return new List<UserInfoAccount>();

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty("accounts", out var accountsElement) ||
                accountsElement.ValueKind != JsonValueKind.Array)
            {
                return new List<UserInfoAccount>();
            }

            var result = new List<UserInfoAccount>();
            foreach (var account in accountsElement.EnumerateArray())
            {
                var accountId = TryGetStringProperty(account, "account_id");
                var baseUri = TryGetStringProperty(account, "base_uri");
                var isDefault = TryGetBooleanLikeProperty(account, "is_default", out var parsed) && parsed;

                if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(baseUri))
                    continue;

                result.Add(new UserInfoAccount
                {
                    AccountId = accountId,
                    BaseUri = baseUri,
                    IsDefault = isDefault
                });
            }

            return result;
        }

        private static string NormalizeAuthServer(string? authServer)
        {
            if (string.IsNullOrWhiteSpace(authServer))
                return "account-d.docusign.com";

            var value = authServer.Trim();
            if (value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                value = value["https://".Length..];
            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                value = value["http://".Length..];

            return value.TrimEnd('/');
        }

        private static bool TryGetBooleanLikeProperty(JsonElement element, string propertyName, out bool value)
        {
            value = false;
            if (!element.TryGetProperty(propertyName, out var prop))
                return false;

            if (prop.ValueKind == JsonValueKind.True)
            {
                value = true;
                return true;
            }

            if (prop.ValueKind == JsonValueKind.False)
            {
                value = false;
                return true;
            }

            if (prop.ValueKind == JsonValueKind.String && bool.TryParse(prop.GetString(), out var parsed))
            {
                value = parsed;
                return true;
            }

            return false;
        }

        private static string? TryGetStringProperty(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return null;

            return prop.ValueKind switch
            {
                JsonValueKind.String => prop.GetString()?.Trim(),
                JsonValueKind.Number => prop.GetRawText(),
                _ => null
            };
        }

        private sealed class UserInfoAccount
        {
            public string AccountId { get; set; } = string.Empty;
            public string BaseUri { get; set; } = string.Empty;
            public bool IsDefault { get; set; }
        }
    }
}
