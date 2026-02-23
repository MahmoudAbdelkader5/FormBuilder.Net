using formBuilder.Domian.Entitys;
using formBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Repositories;
using FormBuilder.Infrastructure.Repository;
using FormBuilder.Services;
using FormBuilder.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.core.Repository
{
    public class UnitOfWork : IunitOfwork, IAsyncDisposable
    {
        private readonly Dictionary<Type, object> _repositories;

        // Private repository fields
        private IFORM_RULESRepository _formRulesRepository;
        private IFormBuilderRepository _formBuilderRepository;
        private IFormTabRepository _formTabRepository;
        private IFormFieldRepository _formFieldRepository;
        private IFieldTypesRepository _fieldTypesRepository;
        private IFieldOptionsRepository _FieldOptionsRepository;
        private IFieldDataSourcesRepository _fieldDataSourcesRepository;
        private IAttachmentTypeRepository _attachmentTypeRepository;
        private IFormAttachmentTypeRepository _formAttachmentTypeRepository;
        private IDocumentTypeRepository _documentTypeRepository;
        private IDocumentSeriesRepository _documentSeriesRepository;
        private IProjectRepository _projectRepository;
        private IFormSubmissionsRepository _formSubmissionsRepository; // تم التصحيح
        private IFormGridColumnRepository _formGridColumnRepository;
        private IGridColumnDataSourcesRepository _gridColumnDataSourcesRepository;
        private IGridColumnOptionsRepository _gridColumnOptionsRepository;
        private readonly FormBuilderDbContext _appDbContext;
        public DbContext AppDbContext => _appDbContext;
        private IFormSubmissionGridRowRepository _formSubmissionGridRowRepository;
        private IFormSubmissionGridCellRepository _formSubmissionGridCellRepository;
        private IFormulasRepository _formulasRepository;
        private IApprovalWorkflowRepository _approvalWorkflowRepository;
        private IFormulaVariableRepository _formulaVariablesRepository;
        private IApprovalStageRepository _approvalStageRepository;
        private IApprovalStageAssigneesRepository _approvalStageAssigneesRepository;
        private IApprovalDelegationRepository _approvalDelegationRepository;
        private IDocumentApprovalHistoryRepository _documentApprovalHistoryRepository;
        private ITableMenusRepository _tableMenusRepository;
        private ITableSubMenusRepository _tableSubMenusRepository;
        private ITableMenuDocumentsRepository _tableMenuDocumentsRepository;
        private IUserQueriesRepository _userQueriesRepository;
        private IAlertRuleRepository _alertRuleRepository;

        public UnitOfWork(FormBuilderDbContext appDbContext)
        {
            _repositories = new Dictionary<Type, object>();
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        // --- Core UoW Methods ---

        public async Task<int> CompleteAsyn()
        {
            return await AppDbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await AppDbContext.DisposeAsync();
        }

        public IBaseRepository<T> Repositary<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repo = new BaseRepository<T>(_appDbContext);
                _repositories.Add(type, repo);
            }
            return (IBaseRepository<T>)_repositories[type];
        }

        // --- Specific Repository Properties ---

        public IFORM_RULESRepository FORM_RULESRepository
        {
            get
            {
                _formRulesRepository ??= new FORM_RULESRepository(_appDbContext);
                return _formRulesRepository;
            }
        }

        public IFormBuilderRepository FormBuilderRepository
        {
            get
            {
                _formBuilderRepository ??= new FormBuilderRepository(_appDbContext);
                return _formBuilderRepository;
            }
        }

        public IFormTabRepository FormTabRepository
        {
            get
            {
                _formTabRepository ??= new FormTabRepository(_appDbContext);
                return _formTabRepository;
            }
        }

        public IFormFieldRepository FormFieldRepository
        {
            get
            {
                _formFieldRepository ??= new FormFieldRepository(_appDbContext);
                return _formFieldRepository;    
            }
        }

        public IFieldTypesRepository FieldTypesRepository
        {
            get
            {
                _fieldTypesRepository ??= new FieldTypesRepository(_appDbContext);
                return _fieldTypesRepository;
            }
        }

        public IFieldOptionsRepository FieldOptionsRepository
        {
            get
            {
                _FieldOptionsRepository ??= new FieldOptionsRepository(_appDbContext);
                return _FieldOptionsRepository;
            }
        }

        public IFieldDataSourcesRepository FieldDataSourcesRepository
        {
            get
            {
                _fieldDataSourcesRepository ??= new FieldDataSourcesRepository(_appDbContext);
                return _fieldDataSourcesRepository;
            }
        }

        public IAttachmentTypeRepository AttachmentTypeRepository
        {
            get
            {
                _attachmentTypeRepository ??= new AttachmentTypeRepository(_appDbContext);
                return _attachmentTypeRepository;
            }
        }

        public IFormAttachmentTypeRepository FormAttachmentTypeRepository
        {
            get
            {
                _formAttachmentTypeRepository ??= new FormAttachmentTypeRepository(_appDbContext);
                return _formAttachmentTypeRepository;
            }
        }

        public IDocumentTypeRepository DocumentTypeRepository
        {
            get
            {
                _documentTypeRepository ??= new DocumentTypeRepository(_appDbContext);
                return _documentTypeRepository;
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                _projectRepository ??= new ProjectRepository(_appDbContext);
                return _projectRepository;
            }
        }

        public IDocumentSeriesRepository DocumentSeriesRepository
        {
            get
            {
                _documentSeriesRepository ??= new DocumentSeriesRepository(_appDbContext);
                return _documentSeriesRepository;
            }
        }

        public IFormSubmissionsRepository FormSubmissionsRepository // تم التصحيح
        {
            get
            {
                _formSubmissionsRepository ??= new FormSubmissionsRepository(_appDbContext);
                return _formSubmissionsRepository;
            }
        }
        private IFormSubmissionValuesRepository _formSubmissionValuesRepository;
        public IFormSubmissionValuesRepository FormSubmissionValuesRepository =>
            _formSubmissionValuesRepository ??= new FormSubmissionValuesRepository(_appDbContext);
        private IFormSubmissionAttachmentsRepository _formSubmissionAttachmentsRepository;
        public IFormSubmissionAttachmentsRepository FormSubmissionAttachmentsRepository =>
            _formSubmissionAttachmentsRepository ??= new FormSubmissionAttachmentsRepository(_appDbContext);
        // Add to UnitOfWork class
        private IFormGridRepository _formGridRepository;

        public IFormGridRepository FormGridRepository
        {
            get
            {
                _formGridRepository ??= new FormGridRepository(_appDbContext);
                return _formGridRepository;
            }
        }

        public IFormGridColumnRepository FormGridColumnRepository
        {
            get
            {
                _formGridColumnRepository ??= new FormGridColumnRepository(_appDbContext);
                return _formGridColumnRepository;
            }
        }

        public IGridColumnDataSourcesRepository GridColumnDataSourcesRepository
        {
            get
            {
                _gridColumnDataSourcesRepository ??= new GridColumnDataSourcesRepository(_appDbContext);
                return _gridColumnDataSourcesRepository;
            }
        }

        public IGridColumnOptionsRepository GridColumnOptionsRepository
        {
            get
            {
                _gridColumnOptionsRepository ??= new GridColumnOptionsRepository(_appDbContext);
                return _gridColumnOptionsRepository;
            }
        }

        public IFormSubmissionGridRowRepository FormSubmissionGridRowRepository
        {
            get
            {
                _formSubmissionGridRowRepository ??= new FormSubmissionGridRowRepository(_appDbContext);
                return _formSubmissionGridRowRepository;
            }
        }
        public IFormSubmissionGridCellRepository FormSubmissionGridCellRepository =>
                _formSubmissionGridCellRepository ??= new FormSubmissionGridCellRepository(_appDbContext);
        public IFormulasRepository FormulasRepository
        {
            get
            {
                _formulasRepository ??= new FormulasRepository(_appDbContext);
                return _formulasRepository;
            }
        }
        public IFormulaVariableRepository FormulaVariablesRepository =>
            _formulaVariablesRepository ??= new FormulaVariablesRepository (_appDbContext);

        public IApprovalStageRepository ApprovalStageRepository
            => _approvalStageRepository ??= new ApprovalStageRepository(_appDbContext);

        public IApprovalStageAssigneesRepository ApprovalStageAssigneesRepository
            => _approvalStageAssigneesRepository ??= new ApprovalStageAssigneesRepository(_appDbContext);

        public IApprovalDelegationRepository ApprovalDelegationRepository
            => _approvalDelegationRepository ??= new ApprovalDelegationRepository(_appDbContext);

        public IDocumentApprovalHistoryRepository DocumentApprovalHistoryRepository
            => _documentApprovalHistoryRepository ??= new DocumentApprovalHistoryRepository(_appDbContext);


        public IApprovalWorkflowRepository ApprovalWorkflowRepository
        {
            get
            {
                if (_approvalWorkflowRepository == null)
                {
                    _approvalWorkflowRepository = new ApprovalWorkflowRepository(_appDbContext);
                }

                return _approvalWorkflowRepository;
            }
        }

        public ITableMenusRepository TableMenusRepository
            => _tableMenusRepository ??= new TableMenusRepository(_appDbContext);

        public ITableSubMenusRepository TableSubMenusRepository
            => _tableSubMenusRepository ??= new TableSubMenusRepository(_appDbContext);

        public ITableMenuDocumentsRepository TableMenuDocumentsRepository
            => _tableMenuDocumentsRepository ??= new TableMenuDocumentsRepository(_appDbContext);

        public IUserQueriesRepository UserQueriesRepository
            => _userQueriesRepository ??= new UserQueriesRepository(_appDbContext);

        public IAlertRuleRepository AlertRuleRepository
            => _alertRuleRepository ??= new AlertRuleRepository(_appDbContext);
    }
}