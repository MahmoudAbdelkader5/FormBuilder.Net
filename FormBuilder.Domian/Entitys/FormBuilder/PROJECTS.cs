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

    [Table("PROJECTS")]
    public class PROJECTS : BaseEntity
    { 
        

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Code { get; set; }

        public string Description { get; set; }
        public new bool IsActive { get; set; }

        public virtual ICollection<DOCUMENT_SERIES> DOCUMENT_SERIES { get; set; }
    }
}
