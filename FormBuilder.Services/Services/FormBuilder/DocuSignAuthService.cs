using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class DocuSignAuthService : IDocuSignAuthService
    {
        private readonly ILogger<DocuSignAuthService> _logger;
        private readonly IConfiguration _configuration;

        public DocuSignAuthService(
            ILogger<DocuSignAuthService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var authServer = GetRequiredSetting("DS_AUTH_SERVER", "DocuSign:OAuthBaseUrl", "DocuSign:OAuthBasePath", "DocuSign:AuthServer");
            var integrationKey = GetRequiredSetting("DS_INTEGRATION_KEY", "DocuSign:IntegratorKey", "DocuSign:ClientId");
            var userId = GetRequiredSetting("DS_USER_ID", "DocuSign:UserId");
            var privateKeyPath = GetRequiredSetting("DS_PRIVATE_KEY_PATH", "DocuSign:PrivateKeyPath");
            var redirectUri = GetOptionalSetting("DS_REDIRECT_URI", "DocuSign:RedirectUri");
            var oauthBasePath = NormalizeOAuthBasePath(authServer);

            if (!File.Exists(privateKeyPath))
                throw new FileNotFoundException($"DocuSign private key file not found at '{privateKeyPath}'.");

            var privateKeyBytes = await File.ReadAllBytesAsync(privateKeyPath);
            var apiClient = new ApiClient();
            try
            {
                apiClient.SetOAuthBasePath(oauthBasePath);
                var response = apiClient.RequestJWTUserToken(
                    integrationKey,
                    userId,
                    oauthBasePath,
                    privateKeyBytes,
                    3600,
                    new List<string> { "signature", "impersonation" });

                var accessToken = NormalizeAccessToken(response?.access_token);
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    var oauthErrorDetails = ExtractOAuthErrorDetails(response);
                    var consentHint = BuildConsentHint(oauthBasePath, integrationKey, redirectUri);
                    throw new InvalidOperationException(
                        "DocuSign JWT authorization returned an empty access token. " +
                        "Verify DS_USER_ID, DS_INTEGRATION_KEY, private key pairing, and user consent. " +
                        $"Auth server: '{oauthBasePath}', User ID: '{userId}', Integration Key: '{integrationKey}'. " +
                        $"{oauthErrorDetails} " +
                        consentHint);
                }

                return accessToken;
            }
            catch (ApiException ex)
            {
                var consentHint = BuildConsentHint(oauthBasePath, integrationKey, redirectUri);
                var statusCode = GetApiExceptionStatusCode(ex);
                object? errorContentObj = ex.ErrorContent;
                var responseBody = errorContentObj?.ToString();
                LoggerExtensions.LogError(_logger, ex,
                    "DocuSign JWT authorization failed. StatusCode: {StatusCode}. Response: {ResponseBody}",
                    statusCode, responseBody ?? string.Empty);
                throw new InvalidOperationException(
                    $"DocuSign JWT authorization failed (HTTP {statusCode}). {consentHint}",
                    ex);
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                var consentHint = BuildConsentHint(oauthBasePath, integrationKey, redirectUri);
                _logger.LogError(ex,
                    "DocuSign JWT authorization failed before token parsing. AuthServer: {AuthServer}, UserId: {UserId}, IntegrationKey: {IntegrationKey}, PrivateKeyPath: {PrivateKeyPath}",
                    oauthBasePath, userId, integrationKey, privateKeyPath);
                throw new InvalidOperationException(
                    "DocuSign JWT authorization failed before token parsing. " +
                    "Verify the private key file format and integration key RSA public key pairing. " +
                    consentHint,
                    ex);
            }
        }

        public async Task<DocuSignAccountInfoDto> GetAccountInfoAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("DocuSign access token is required.", nameof(accessToken));

            var normalizedAccessToken = NormalizeAccessToken(accessToken);
            if (string.IsNullOrWhiteSpace(normalizedAccessToken))
                throw new ArgumentException("DocuSign access token is invalid.", nameof(accessToken));

            try
            {
                var endpoint = BuildUserInfoEndpoint();

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", normalizedAccessToken);
                var response = await httpClient.GetAsync(endpoint);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning(
                        "DocuSign user info request was unauthorized. StatusCode: {StatusCode}. Response: {ResponseBody}",
                        (int)response.StatusCode,
                        responseBody);

                    throw new InvalidOperationException(
                        "DocuSign user info request failed (HTTP 401). Verify the access token is valid and not expired.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        "DocuSign user info request failed. StatusCode: {StatusCode}. Response: {ResponseBody}",
                        (int)response.StatusCode,
                        responseBody);

                    throw new InvalidOperationException(
                        $"DocuSign user info request failed (HTTP {(int)response.StatusCode}). Verify token and account access.");
                }

                var preferredAccountId = GetOptionalSetting("DS_ACCOUNT_ID", "DocuSign:AccountId");
                var authServer = GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:OAuthBaseUrl")
                                 ?? GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:OAuthBasePath")
                                 ?? GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:AuthServer")
                                 ?? "account-d.docusign.com";
                var (accountId, baseUri) = ParseAccountFromUserInfoJson(responseBody, preferredAccountId, authServer);
                if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(baseUri))
                    throw new InvalidOperationException("Unable to resolve DocuSign account info from user info endpoint.");

                return new DocuSignAccountInfoDto
                {
                    AccountId = accountId,
                    BaseUri = baseUri
                };
            }
            catch (ApiException ex)
            {
                var statusCode = GetApiExceptionStatusCode(ex);
                string responseBody = Convert.ToString(ex.ErrorContent) ?? string.Empty;
                _logger.LogError(ex,
                    "DocuSign user info fetch failed. StatusCode: {StatusCode}. Response: {ResponseBody}",
                    statusCode, responseBody);

                throw new InvalidOperationException(
                    $"DocuSign user info request failed (HTTP {statusCode}). Verify the access token is valid and not expired.",
                    ex);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "DocuSign user info parsing failed due to an unexpected status/content format.");
                throw new InvalidOperationException(
                    "DocuSign user info request failed. The response format was unexpected, often caused by unauthorized or expired token.",
                    ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "DocuSign user info JSON parsing failed.");
                throw new InvalidOperationException(
                    "DocuSign user info request failed due to invalid response format.",
                    ex);
            }
        }

        private static string NormalizePrivateKey(string pem)
        {
            if (string.IsNullOrWhiteSpace(pem))
                return pem;

            return pem.Trim().TrimStart('\uFEFF');
        }

        private static string NormalizeOAuthBasePath(string value)
        {
            var v = value.Trim();
            if (v.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                v = v["https://".Length..];
            if (v.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                v = v["http://".Length..];
            return v.TrimEnd('/');
        }

        private static string BuildConsentHint(string authServer, string integrationKey, string? redirectUri)
        {
            if (string.IsNullOrWhiteSpace(integrationKey) || string.IsNullOrWhiteSpace(redirectUri))
                return "Grant consent for scopes 'signature impersonation' in DocuSign for this integration key.";

            var encodedRedirect = Uri.EscapeDataString(redirectUri);
            var consentUrl =
                $"https://{authServer}/oauth/auth?response_type=code&scope=signature%20impersonation&client_id={integrationKey}&redirect_uri={encodedRedirect}";
            return $"Grant consent once using: {consentUrl}";
        }

        private string GetRequiredSetting(string envKey, params string[] configKeys)
        {
            var value = Environment.GetEnvironmentVariable(envKey);
            if (string.IsNullOrWhiteSpace(value))
            {
                foreach (var key in configKeys)
                {
                    value = _configuration[key];
                    if (!string.IsNullOrWhiteSpace(value))
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogError(
                    "Missing DocuSign setting. Env key: {EnvKey}, Config keys: {ConfigKeys}",
                    envKey,
                    string.Join(", ", configKeys));
                throw new InvalidOperationException(
                    $"Missing DocuSign setting. Set either environment variable '{envKey}' or one of appsettings keys: {string.Join(", ", configKeys)}.");
            }

            return value.Trim();
        }

        private string? GetOptionalSetting(string envKey, string configKey)
        {
            var value = Environment.GetEnvironmentVariable(envKey);
            if (string.IsNullOrWhiteSpace(value))
                value = _configuration[configKey];

            return value?.Trim();
        }

        private static string? NormalizeAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return token;

            var trimmed = token.Trim();
            const string bearerPrefix = "Bearer ";
            if (trimmed.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                return trimmed[bearerPrefix.Length..].Trim();

            return trimmed;
        }

        private static int GetApiExceptionStatusCode(ApiException ex)
        {
            var code = ex.ErrorCode.ToString();
            return int.TryParse(code, out var parsedCode) ? parsedCode : 0;
        }

        private static string ExtractOAuthErrorDetails(object? tokenResponse)
        {
            if (tokenResponse == null)
                return string.Empty;

            try
            {
                var type = tokenResponse.GetType();
                var error = type.GetProperty("error")?.GetValue(tokenResponse)?.ToString()
                            ?? type.GetProperty("Error")?.GetValue(tokenResponse)?.ToString();
                var description = type.GetProperty("error_description")?.GetValue(tokenResponse)?.ToString()
                                  ?? type.GetProperty("ErrorDescription")?.GetValue(tokenResponse)?.ToString();

                if (string.IsNullOrWhiteSpace(error) && string.IsNullOrWhiteSpace(description))
                    return string.Empty;

                return $"OAuth error: '{error ?? "unknown"}'. Description: '{description ?? "n/a"}'.";
            }
            catch
            {
                return string.Empty;
            }
        }

        private string BuildUserInfoEndpoint()
        {
            var authServer = GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:OAuthBaseUrl")
                             ?? GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:OAuthBasePath")
                             ?? GetOptionalSetting("DS_AUTH_SERVER", "DocuSign:AuthServer")
                             ?? "account-d.docusign.com";

            var normalizedAuthServer = NormalizeOAuthBasePath(authServer);
            return $"https://{normalizedAuthServer}/oauth/userinfo";
        }

        private static (string? accountId, string? baseUri) ParseAccountFromUserInfoJson(
            string json,
            string? preferredAccountId = null,
            string? authServer = null)
        {
            using var document = JsonDocument.Parse(json);
            if (!document.RootElement.TryGetProperty("accounts", out var accountsElement) ||
                accountsElement.ValueKind != JsonValueKind.Array)
            {
                return (null, null);
            }

            var accounts = accountsElement
                .EnumerateArray()
                .Select(a => new
                {
                    AccountId = TryGetStringProperty(a, "account_id"),
                    BaseUri = TryGetStringProperty(a, "base_uri"),
                    IsDefault = TryGetBooleanLikeProperty(a, "is_default", out var isDefault) && isDefault
                })
                .Where(a => !string.IsNullOrWhiteSpace(a.AccountId) && !string.IsNullOrWhiteSpace(a.BaseUri))
                .ToList();

            if (accounts.Count == 0)
                return (null, null);

            if (!string.IsNullOrWhiteSpace(preferredAccountId))
            {
                var preferred = preferredAccountId.Trim();
                var preferredMatch = accounts.FirstOrDefault(a =>
                    string.Equals(a.AccountId, preferred, StringComparison.OrdinalIgnoreCase));
                if (preferredMatch != null)
                    return (preferredMatch.AccountId, preferredMatch.BaseUri);
            }

            // When using Demo OAuth host, force selecting a Demo account if available.
            if (!string.IsNullOrWhiteSpace(authServer) &&
                NormalizeOAuthBasePath(authServer).Contains("account-d.", StringComparison.OrdinalIgnoreCase))
            {
                var demoMatch = accounts.FirstOrDefault(a =>
                    a.BaseUri != null &&
                    a.BaseUri.Contains("demo.docusign.net", StringComparison.OrdinalIgnoreCase));
                if (demoMatch != null)
                    return (demoMatch.AccountId, demoMatch.BaseUri);
            }

            var defaultMatch = accounts.FirstOrDefault(a => a.IsDefault);
            if (defaultMatch != null)
                return (defaultMatch.AccountId, defaultMatch.BaseUri);

            return (accounts[0].AccountId, accounts[0].BaseUri);
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

            if (prop.ValueKind == JsonValueKind.String)
            {
                var text = prop.GetString();
                if (bool.TryParse(text, out var parsedBool))
                {
                    value = parsedBool;
                    return true;
                }
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

    }
}
