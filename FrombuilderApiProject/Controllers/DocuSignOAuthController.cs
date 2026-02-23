using DocuSign.eSign.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/docusign-oauth")]
    public class DocuSignOAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DocuSignOAuthController> _logger;

        public DocuSignOAuthController(
            IConfiguration configuration,
            ILogger<DocuSignOAuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string scope = "signature", [FromQuery] string? state = null)
        {
            try
            {
                var clientId = GetRequiredSetting("DS_INTEGRATION_KEY", "DocuSign:ClientId", "DocuSign:IntegratorKey");
                var oauthBasePath = NormalizeOAuthBasePath(
                    GetRequiredSetting("DS_AUTH_SERVER", "DocuSign:OAuthBasePath", "DocuSign:OAuthBaseUrl", "DocuSign:AuthServer"));
                var redirectUri = GetRequiredSetting("DS_REDIRECT_URI", "DocuSign:RedirectUri");

                var encodedRedirectUri = Uri.EscapeDataString(redirectUri);
                var encodedScope = Uri.EscapeDataString(string.IsNullOrWhiteSpace(scope) ? "signature" : scope.Trim());
                var stateValue = Uri.EscapeDataString(string.IsNullOrWhiteSpace(state) ? Guid.NewGuid().ToString("N") : state.Trim());

                var url =
                    $"https://{oauthBasePath}/oauth/auth" +
                    $"?response_type=code" +
                    $"&scope={encodedScope}" +
                    $"&client_id={clientId}" +
                    $"&redirect_uri={encodedRedirectUri}" +
                    $"&state={stateValue}";

                return Redirect(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DocuSign OAuth login URL generation failed.");
                return BadRequest(new { ok = false, error = ex.Message });
            }
        }

        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] string? code = null, [FromQuery] string? state = null, [FromQuery] string? error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
                return BadRequest(new { ok = false, error, state });

            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new { ok = false, error = "Missing authorization code.", state });

            try
            {
                var clientId = GetRequiredSetting("DS_INTEGRATION_KEY", "DocuSign:ClientId", "DocuSign:IntegratorKey");
                var clientSecret = GetRequiredSetting("DS_CLIENT_SECRET", "DocuSign:ClientSecret");
                var apiBasePath = GetOptionalSetting("DS_API_BASE_PATH", "DocuSign:ApiBasePath", "DocuSign:BaseUrl")
                                  ?? "https://demo.docusign.net/restapi";
                var oauthBasePath = NormalizeOAuthBasePath(
                    GetRequiredSetting("DS_AUTH_SERVER", "DocuSign:OAuthBasePath", "DocuSign:OAuthBaseUrl", "DocuSign:AuthServer"));

                var apiClient = new ApiClient();
                apiClient.SetBasePath(apiBasePath);

                var token = apiClient.GenerateAccessToken(clientId, clientSecret, code.Trim());
                var accessToken = token?.access_token;
                if (string.IsNullOrWhiteSpace(accessToken))
                    return StatusCode(500, new { ok = false, error = "DocuSign returned empty access token from authorization code exchange." });

                var userInfoEndpoint = $"https://{oauthBasePath}/oauth/userinfo";
                var account = ResolveAccountFromUserInfo(accessToken, userInfoEndpoint);

                return Ok(new
                {
                    ok = true,
                    state,
                    token = new
                    {
                        accessToken = accessToken,
                        refreshToken = token?.refresh_token,
                        expiresIn = token?.expires_in
                    },
                    account = new
                    {
                        accountId = account.accountId,
                        baseUri = account.baseUri
                    }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "DocuSign OAuth callback unauthorized while fetching user info.");
                return Unauthorized(new
                {
                    ok = false,
                    error = "DocuSign user info request failed (HTTP 401). Verify the access token is valid and not expired.",
                    state
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DocuSign OAuth callback failed.");
                return StatusCode(500, new { ok = false, error = ex.Message, state });
            }
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
                throw new InvalidOperationException(
                    $"Missing DocuSign setting. Set '{envKey}' or one of: {string.Join(", ", configKeys)}.");

            return value.Trim();
        }

        private string? GetOptionalSetting(string envKey, params string[] configKeys)
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

            return value?.Trim();
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

        private static (string? accountId, string? baseUri) ResolveAccountFromUserInfo(string accessToken, string userInfoEndpoint)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = httpClient.GetAsync(userInfoEndpoint).GetAwaiter().GetResult();
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Unauthorized while fetching DocuSign user info.");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"DocuSign user info request failed (HTTP {(int)response.StatusCode}).");

            using var document = JsonDocument.Parse(body);
            if (!document.RootElement.TryGetProperty("accounts", out var accountsElement) ||
                accountsElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Unable to resolve DocuSign account info from user info endpoint.");
            }

            JsonElement? selected = null;
            foreach (var account in accountsElement.EnumerateArray())
            {
                if (selected == null)
                    selected = account;

                if (TryGetBooleanLikeProperty(account, "is_default", out var isDefault) && isDefault)
                {
                    selected = account;
                    break;
                }
            }

            if (selected == null)
                throw new InvalidOperationException("Unable to resolve DocuSign account info from user info endpoint.");

            return (
                TryGetStringProperty(selected.Value, "account_id"),
                TryGetStringProperty(selected.Value, "base_uri"));
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
