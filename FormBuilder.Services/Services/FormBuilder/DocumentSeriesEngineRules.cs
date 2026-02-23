using FormBuilder.Domian.Entitys.FromBuilder;
using System.Text.RegularExpressions;

namespace FormBuilder.Services.Services.FormBuilder
{
    internal static class DocumentSeriesEngineRules
    {
        private static readonly HashSet<string> SupportedPlaceholders = new(StringComparer.OrdinalIgnoreCase)
        {
            "PROJECT",
            "YYYY",
            "MM",
            "DD",
            "SEQ"
        };

        private static readonly Regex PlaceholderRegex = new(@"\{([A-Z]+)\}", RegexOptions.Compiled);

        public static bool TryNormalizeResetPolicy(string? resetPolicy, out string normalized)
        {
            normalized = (resetPolicy ?? "None").Trim();
            if (normalized.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "None";
                return true;
            }

            if (normalized.Equals("Yearly", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Yearly";
                return true;
            }

            if (normalized.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Monthly";
                return true;
            }

            if (normalized.Equals("Daily", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Daily";
                return true;
            }

            return false;
        }

        public static bool TryNormalizeGenerateOn(string? generateOn, out string normalized)
        {
            normalized = (generateOn ?? "Submit").Trim();
            if (normalized.Equals("Submit", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Submit";
                return true;
            }

            if (normalized.Equals("Approval", StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Approval";
                return true;
            }

            return false;
        }

        public static bool TryValidateTemplate(string? template, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(template))
            {
                errorMessage = "Template is required.";
                return false;
            }

            var matches = PlaceholderRegex.Matches(template);
            foreach (Match match in matches)
            {
                var token = match.Groups[1].Value;
                if (!SupportedPlaceholders.Contains(token))
                {
                    errorMessage = $"Unsupported placeholder '{{{token}}}'. Supported placeholders: {{PROJECT}}, {{YYYY}}, {{MM}}, {{DD}}, {{SEQ}}.";
                    return false;
                }
            }

            if (!template.Contains("{SEQ}", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Template must include {SEQ}.";
                return false;
            }

            return true;
        }

        public static string BuildPeriodKey(string resetPolicy, DateTime utcNow)
        {
            return resetPolicy switch
            {
                "Yearly" => utcNow.ToString("yyyy"),
                "Monthly" => utcNow.ToString("yyyyMM"),
                "Daily" => utcNow.ToString("yyyyMMdd"),
                _ => "GLOBAL"
            };
        }

        public static string RenderTemplate(
            DOCUMENT_SERIES series,
            string projectCode,
            DateTime utcNow,
            int sequenceNumber)
        {
            var template = string.IsNullOrWhiteSpace(series.Template)
                ? $"{series.SeriesCode}-{{SEQ}}"
                : series.Template;

            var padding = series.SequencePadding <= 0 ? 3 : series.SequencePadding;
            var seq = sequenceNumber.ToString($"D{padding}");

            return template
                .Replace("{PROJECT}", NormalizeProjectToken(projectCode), StringComparison.OrdinalIgnoreCase)
                .Replace("{YYYY}", utcNow.ToString("yyyy"), StringComparison.OrdinalIgnoreCase)
                .Replace("{MM}", utcNow.ToString("MM"), StringComparison.OrdinalIgnoreCase)
                .Replace("{DD}", utcNow.ToString("dd"), StringComparison.OrdinalIgnoreCase)
                .Replace("{SEQ}", seq, StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeProjectToken(string projectCode)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
                return "NA";

            var filtered = new string(projectCode
                .Where(char.IsLetterOrDigit)
                .ToArray());

            return string.IsNullOrWhiteSpace(filtered)
                ? "NA"
                : filtered.ToUpperInvariant();
        }

        public static bool IsDraftDocumentNumber(string? documentNumber)
        {
            return string.IsNullOrWhiteSpace(documentNumber)
                || documentNumber.StartsWith("DRAFT-", StringComparison.OrdinalIgnoreCase);
        }
    }
}
