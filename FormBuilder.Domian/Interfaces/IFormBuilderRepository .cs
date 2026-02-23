using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using formBuilder.Domian.Interfaces;
using global::FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Domian.Interfaces
{
    public interface IFormBuilderRepository : IBaseRepository<FORM_BUILDER>
    {
        // يمكنك إضافة أي عمليات خاصة بالـ FormBuilder هنا
        Task<bool> IsFormCodeExistsAsync(string formCode, int? excludeId = null);

        /// <summary>
        /// Gets a form by code including its tabs and fields (for public/anonymous view).
        /// </summary>
        Task<FORM_BUILDER?> GetFormWithTabsAndFieldsByCodeAsync(string formCode);

        /// <summary>
        /// Gets a form by ID including all related data (tabs, fields, options, data sources, formulas, rules) for duplication.
        /// </summary>
        Task<FORM_BUILDER?> GetFormWithAllDataForDuplicateAsync(int id);
    }
}


