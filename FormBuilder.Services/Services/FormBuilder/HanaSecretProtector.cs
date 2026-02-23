using Microsoft.AspNetCore.DataProtection;

namespace FormBuilder.Services.Services.FormBuilder
{
    public interface IHanaSecretProtector
    {
        string Protect(string plaintext);
        string Unprotect(string protectedText);
    }

    /// <summary>
    /// Separate purpose string from SMTP encryption so we don't break existing encrypted values.
    /// </summary>
    public class HanaSecretProtector : IHanaSecretProtector
    {
        private const string Purpose = "FormBuilder.SapHana.ConnectionString";
        private readonly IDataProtector _protector;

        public HanaSecretProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(Purpose);
        }

        public string Protect(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return string.Empty;
            return _protector.Protect(plaintext);
        }

        public string Unprotect(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText)) return string.Empty;
            return _protector.Unprotect(protectedText);
        }
    }
}


