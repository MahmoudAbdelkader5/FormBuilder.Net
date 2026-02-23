using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder; // المسار الصحيح لكيان FORM_TABS
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface IFormTabRepository : IBaseRepository<FORM_TABS>
    {
        /// <summary>
        /// التحقق مما إذا كان TabCode فريدًا داخل FormBuilder معيّن (مع استثناء سجل معين أثناء التحديث).
        /// </summary>
        /// <param name="formBuilderId">معرف الـ FormBuilder الأب.</param>
        /// <param name="tabCode">الكود المراد التحقق منه.</param>
        /// <param name="excludeId">المعرف المراد استثناؤه من البحث (لعمليات التحديث).</param>
        Task<bool> IsTabCodeUniqueAsync(int formBuilderId, string tabCode, int? excludeId = null);

        /// <summary>
        /// التحقق مما إذا كان TabCode موجود في الجدول (على مستوى كل الـ Forms)
        /// </summary>
        /// <param name="tabCode">الكود المراد التحقق منه.</param>
        /// <param name="excludeId">المعرف المراد استثناؤه من البحث.</param>
        Task<bool> TabCodeExistsAsync(string tabCode, int? excludeId = null);

        /// <summary>
        /// استرداد جميع التبويبات المرتبطة بـ FormBuilder محدد باستخدام معرفه.
        /// </summary>
        /// <param name="formBuilderId">معرف النموذج الأب (FormBuilder).</param>
        Task<IEnumerable<FORM_TABS>> GetTabsByFormIdAsync(int formBuilderId);
    }
}