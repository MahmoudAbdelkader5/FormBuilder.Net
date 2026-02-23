using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FromBuilder
{

    [Table("SMTP_CONFIGS")]
    public class SMTP_CONFIGS : BaseEntity
    {
       

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Host { get; set; }

        public int Port { get; set; }
        public bool UseSsl { get; set; }

        [Required, StringLength(200)]
        public string UserName { get; set; }

        [Required, StringLength(500)]
        public string PasswordEncrypted { get; set; }

        [Required, StringLength(200)]
        public string FromEmail { get; set; }

        [Required, StringLength(200)]
        public string FromDisplayName { get; set; }

        public new bool IsActive { get; set; }

        public virtual ICollection<EMAIL_TEMPLATES> EMAIL_TEMPLATES { get; set; }
    }

}
