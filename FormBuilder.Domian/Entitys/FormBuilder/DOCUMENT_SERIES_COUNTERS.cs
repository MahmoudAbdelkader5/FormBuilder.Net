using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("DOCUMENT_SERIES_COUNTERS")]
    public class DOCUMENT_SERIES_COUNTERS : BaseEntity
    {
        [ForeignKey("DOCUMENT_SERIES")]
        public int SeriesId { get; set; }
        public virtual DOCUMENT_SERIES DOCUMENT_SERIES { get; set; }

        [Required, StringLength(50)]
        public string PeriodKey { get; set; }  // e.g., "2025", "202502", "20250209"

        public int CurrentNumber { get; set; }
    }
}
