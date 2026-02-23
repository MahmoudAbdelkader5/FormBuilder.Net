using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FromBuilder
{
    [Table("SAP_HANA_CONFIGS")]
    public class SAP_HANA_CONFIGS : BaseEntity
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Encrypted using ASP.NET Core DataProtection (see IHanaSecretProtector).
        /// </summary>
        [Required, StringLength(2000)]
        public string ConnectionStringEncrypted { get; set; } = string.Empty;

        public new bool IsActive { get; set; }
    }
}
