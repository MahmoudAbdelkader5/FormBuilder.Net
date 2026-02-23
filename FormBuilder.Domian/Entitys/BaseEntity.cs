using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formBuilder.Domian.Entitys
{
    public class BaseEntity
    {
        public int Id { get; set; }
        // يمكنك إضافة حقول التدقيق المشتركة هنا
        // ✅ هذه يجب أن تكون الأسماء الصحيحة المطابقة لقاعدة البيانات
        [StringLength(450)]
        public string ?CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Soft Delete flag - للتمييز بين المعطل (IsActive = false) والمحذوف (IsDeleted = true)
        public bool IsDeleted { get; set; } = false;

        // تاريخ الحذف (Soft Delete)
        public DateTime? DeletedDate { get; set; }

        // من قام بالحذف
        [StringLength(450)]
        public string? DeletedByUserId { get; set; }
    }
}
