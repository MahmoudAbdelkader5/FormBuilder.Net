using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class SapHanaConfigDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Connection kind: "HanaOdbc" or "ServiceLayer"
        public string IntegrationType { get; set; } = "HanaOdbc";

        // Non-secret metadata (derived from decrypted connection string)
        public string? Server { get; set; }
        public string? UserName { get; set; }
        public string? Schema { get; set; }
        public int? MaxPoolSize { get; set; }

        // SAP Service Layer metadata (non-secret)
        public string? BaseUrl { get; set; }
        public string? AuthenticationMethod { get; set; } // Session / Token
        public string? CompanyDb { get; set; }
        public bool VerifySsl { get; set; } = true;

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class CreateSapHanaConfigDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = "Default";

        /// <summary>
        /// "HanaOdbc" or "ServiceLayer".
        /// Defaults to "ServiceLayer" to support SAP Business One Service Layer scenarios.
        /// </summary>
        public string IntegrationType { get; set; } = "ServiceLayer";

        /// <summary>
        /// Full connection string (optional). If omitted, Server/UserName/Password/Schema will be used to build it.
        /// </summary>
        public string? ConnectionString { get; set; }

        public string? Server { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Schema { get; set; }
        public int? MaxPoolSize { get; set; }

        // Service Layer fields
        public string? BaseUrl { get; set; }
        public string AuthenticationMethod { get; set; } = "Session"; // Session / Token
        public string? CompanyDb { get; set; }
        public bool VerifySsl { get; set; } = true;

        public bool IsActive { get; set; } = false;
    }

    public class UpdateSapHanaConfigDto
    {
        [StringLength(200)]
        public string? Name { get; set; }

        /// <summary>
        /// "HanaOdbc" or "ServiceLayer"
        /// </summary>
        public string? IntegrationType { get; set; }

        /// <summary>
        /// Replace full connection string (optional). If omitted, split fields will be used (when provided).
        /// If neither provided, only non-secret fields like Name/IsActive will be updated.
        /// </summary>
        public string? ConnectionString { get; set; }

        public string? Server { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Schema { get; set; }
        public int? MaxPoolSize { get; set; }

        // Service Layer fields
        public string? BaseUrl { get; set; }
        public string? AuthenticationMethod { get; set; } // Session / Token
        public string? CompanyDb { get; set; }
        public bool? VerifySsl { get; set; }

        public bool? IsActive { get; set; }
    }
}

