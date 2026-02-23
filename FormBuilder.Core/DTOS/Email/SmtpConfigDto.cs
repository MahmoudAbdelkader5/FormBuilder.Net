using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.Models.DTOs
{
    public class SmtpConfigDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool HasPassword { get; set; }
        public string FromEmail { get; set; } = string.Empty;
        public string FromDisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateSmtpConfigDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Host { get; set; } = string.Empty;

        [Range(1, 65535)]
        public int Port { get; set; } = 587;

        public bool UseSsl { get; set; } = true;

        [Required, StringLength(200)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Plain password provided by client. Will be encrypted before saving.
        /// </summary>
        [Required, StringLength(500)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string FromEmail { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string FromDisplayName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateSmtpConfigDto
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Host { get; set; }

        [Range(1, 65535)]
        public int? Port { get; set; }

        public bool? UseSsl { get; set; }

        [StringLength(200)]
        public string? UserName { get; set; }

        /// <summary>
        /// Optional: if provided, replaces stored password.
        /// </summary>
        [StringLength(500)]
        public string? Password { get; set; }

        [StringLength(200)]
        public string? FromEmail { get; set; }

        [StringLength(200)]
        public string? FromDisplayName { get; set; }

        public bool? IsActive { get; set; }
    }
}


