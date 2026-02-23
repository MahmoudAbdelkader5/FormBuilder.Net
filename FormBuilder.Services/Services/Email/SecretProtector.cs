using Microsoft.AspNetCore.DataProtection;

namespace FormBuilder.Services.Services.Email
{
    public interface ISecretProtector
    {
        string Protect(string plaintext);
        string Unprotect(string protectedText);
    }

    public class SecretProtector : ISecretProtector
    {
        private const string Purpose = "FormBuilder.Smtp.Password";
        private readonly IDataProtector _protector;

        public SecretProtector(IDataProtectionProvider provider)
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


