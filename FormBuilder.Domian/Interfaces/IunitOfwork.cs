using formBuilder.Domian.Entitys;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formBuilder.Domian.Interfaces
{
    public interface IunitOfwork : IAsyncDisposable
    {
        // Core UoW methods
        Task<int> CompleteAsyn();
        IBaseRepository<T> Repositary<T>() where T : BaseEntity;
        
        // DbContext access for raw SQL operations
        DbContext AppDbContext { get; }

        // Specific repositories
        IFormBuilderRepository FormBuilderRepository { get; }
        IFormTabRepository FormTabRepository { get; }
        IFormFieldRepository FormFieldRepository { get; }
        IFieldTypesRepository FieldTypesRepository { get; }
        IFORM_RULESRepository FORM_RULESRepository { get; }
        IFieldOptionsRepository FieldOptionsRepository { get; }
        IFieldDataSourcesRepository FieldDataSourcesRepository { get; }
        IAttachmentTypeRepository AttachmentTypeRepository { get; }
        IFormAttachmentTypeRepository FormAttachmentTypeRepository { get; } // Added
        IDocumentTypeRepository DocumentTypeRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IDocumentSeriesRepository DocumentSeriesRepository { get; }
        IFormSubmissionsRepository FormSubmissionsRepository { get; } 
        IFormSubmissionValuesRepository FormSubmissionValuesRepository { get; } 
        IFormSubmissionAttachmentsRepository FormSubmissionAttachmentsRepository { get; }
        IFormGridRepository FormGridRepository { get; }
        IFormGridColumnRepository FormGridColumnRepository { get; }
        IGridColumnDataSourcesRepository GridColumnDataSourcesRepository { get; }
        IGridColumnOptionsRepository GridColumnOptionsRepository { get; }
        IFormSubmissionGridRowRepository FormSubmissionGridRowRepository { get; }
        IFormSubmissionGridCellRepository FormSubmissionGridCellRepository { get; }

        // Add these two new repositories for Formulas
        IFormulasRepository FormulasRepository { get; }
        //IFormulaVariablesRepository FormulaVariablesRepository { get; }
        IApprovalWorkflowRepository ApprovalWorkflowRepository { get; }
        IApprovalStageRepository ApprovalStageRepository { get; }
        IApprovalStageAssigneesRepository ApprovalStageAssigneesRepository { get; }
        IApprovalDelegationRepository ApprovalDelegationRepository { get; }
        IDocumentApprovalHistoryRepository DocumentApprovalHistoryRepository { get; }
        IFormulaVariableRepository FormulaVariablesRepository { get; }

        // Table Menus Repositories
        ITableMenusRepository TableMenusRepository { get; }
        ITableSubMenusRepository TableSubMenusRepository { get; }
        ITableMenuDocumentsRepository TableMenuDocumentsRepository { get; }

        // User Queries Repository
        IUserQueriesRepository UserQueriesRepository { get; }

        // Alert Rules Repository
        IAlertRuleRepository AlertRuleRepository { get; }







    }
}