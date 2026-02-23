using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormBuilder.Domian.Entitys.FormBuilder;


// افترض أنك تستخدم هذا المسار لطبقة البنية التحتية
namespace FormBuilder.Infrastructure.Repository
    {
        // يجب أن يرث من تطبيق القاعدة العامة (BaseRepository) والواجهة المخصصة
        public class FormTabRepository : BaseRepository<Domian.Entitys.FormBuilder.FORM_TABS>, IFormTabRepository
        {
            // الوصول إلى DbContext مباشرة لتنفيذ الاستعلامات المعقدة
            private readonly FormBuilderDbContext _context;

            public FormTabRepository(FormBuilderDbContext context) : base(context)
            {
                _context = context;
            }

            // --- تنفيذ الوظائف المخصصة من IFormTabRepository ---

            /// <summary>
            /// استرداد جميع التبويبات المرتبطة بـ FormBuilder محدد.
            /// </summary>
            public async Task<IEnumerable<FORM_TABS>> GetTabsByFormIdAsync(int formBuilderId)
            {
                // يجلب التبويبات التابعة لنموذج معين ويرتبها حسب TabOrder (باستثناء المحذوفة)
                return await _context.FORM_TABS
                                     .Where(t => t.FormBuilderId == formBuilderId && !t.IsDeleted)
                                     .OrderBy(t => t.TabOrder)
                                     .ToListAsync();
            }

            /// <summary>
            /// التحقق مما إذا كان TabCode فريداً داخل FormBuilder معيّن (غير مستخدم من قبل).
            /// يعيد true إذا كان الكود متاحاً / غير موجود في الجدول، و false إذا كان مستخدماً.
            /// </summary>
            public async Task<bool> IsTabCodeUniqueAsync(int formBuilderId, string tabCode, int? excludeId = null)
            {
                if (string.IsNullOrEmpty(tabCode))
                {
                    // نعتبره غير صالح => ليس فريداً
                    return false;
                }

                // يبدأ الاستعلام بالبحث عن أي سجل يطابق TabCode داخل نفس الـ FormBuilder ويتجاهل المحذوفات
                var query = _context.FORM_TABS.Where(t =>
                    t.FormBuilderId == formBuilderId &&
                    t.TabCode == tabCode &&
                    !t.IsDeleted);

                // إذا تم توفير excludeId (لعمليات التحديث)، يتم استبعاد هذا المعرف من البحث
                if (excludeId.HasValue)
                {
                    query = query.Where(t => t.Id != excludeId.Value);
                }

                // إذا وُجد أي سجل يطابق الشروط فهذا يعني أن الكود "غير فريد"
                var exists = await query.AnyAsync();

                // نعكس النتيجة لأن اسم الدالة يشير إلى "فريد"
                return !exists;
            }

            /// <summary>
            /// التحقق مما إذا كان TabCode موجود في الجدول (على مستوى كل الـ Forms)
            /// يعيد true إذا كان الكود موجود، و false إذا كان غير موجود
            /// </summary>
            public async Task<bool> TabCodeExistsAsync(string tabCode, int? excludeId = null)
            {
                if (string.IsNullOrEmpty(tabCode))
                {
                    return false;
                }

                var query = _context.FORM_TABS.Where(t => t.TabCode == tabCode && !t.IsDeleted);

                if (excludeId.HasValue)
                {
                    query = query.Where(t => t.Id != excludeId.Value);
                }

                return await query.AnyAsync();
            }
        }
    }

