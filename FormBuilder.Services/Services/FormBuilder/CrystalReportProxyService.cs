using System.Net.Http.Headers;
using System.Text;
using FormBuilder.Core.Configuration;
using FormBuilder.Core.IServices;
using Microsoft.Extensions.Options;

namespace FormBuilder.Services.Services.FormBuilder;

public sealed class CrystalReportProxyService : ICrystalReportProxyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CrystalBridgeOptions _options;

    public CrystalReportProxyService(IHttpClientFactory httpClientFactory, IOptions<CrystalBridgeOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<(byte[] Content, string ContentType, string? FileName)> GenerateLayoutPdfAsync(
        int idLayout,
        int idObject,
        string fileName,
        string? printedByUserId = null,
        CancellationToken cancellationToken = default)
    {
        if (idLayout <= 0)
            throw new ArgumentOutOfRangeException(nameof(idLayout));

        if (idObject <= 0)
            throw new ArgumentOutOfRangeException(nameof(idObject));

        var safeFileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
        var generateLayoutPath = string.IsNullOrWhiteSpace(_options.GenerateLayoutPath)
            ? "api/reports/GenerateLayout"
            : _options.GenerateLayoutPath.Trim();
        var requestPath =
            $"{generateLayoutPath}?idLayout={idLayout}&idObject={idObject}&fileName={Uri.EscapeDataString(safeFileName)}";

        if (!string.IsNullOrWhiteSpace(printedByUserId))
            requestPath += $"&printedByUserId={Uri.EscapeDataString(printedByUserId)}";

        var client = _httpClientFactory.CreateClient("CrystalBridge");
        if (client.BaseAddress == null && !Uri.TryCreate(requestPath, UriKind.Absolute, out _))
        {
            throw new InvalidOperationException(
                "CrystalBridge BaseUrl is not configured. Set CrystalBridge:BaseUrl in appsettings (e.g. http://localhost:5005/).");
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

        if (!string.IsNullOrWhiteSpace(_options.Username))
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.Username}:{_options.Password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
        }

        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                $"Unable to reach Crystal bridge at '{client.BaseAddress}'. Ensure the bridge service is running and reachable.",
                ex);
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorText = (await response.Content.ReadAsStringAsync(cancellationToken))?.Trim();
                var bridgeMessage = string.IsNullOrWhiteSpace(errorText)
                    ? $"Crystal bridge request failed with HTTP {(int)response.StatusCode}."
                    : $"Crystal bridge request failed with HTTP {(int)response.StatusCode}: {errorText}";

                throw new InvalidOperationException(bridgeMessage);
            }

            var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/pdf";
            var downloadedFileName = response.Content.Headers.ContentDisposition?.FileNameStar
                ?? response.Content.Headers.ContentDisposition?.FileName;

            if (!string.IsNullOrWhiteSpace(downloadedFileName))
                downloadedFileName = downloadedFileName.Trim('"');

            return (content, contentType, downloadedFileName);
        }
    }
}
