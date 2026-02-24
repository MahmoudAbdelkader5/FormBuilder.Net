using formBuilder.Domian.Interfaces;
using AutoMapper;
using FluentValidation;
using FormBuilder.Application.Abstractions;
using FormBuilder.core.Repository;
using FormBuilder.Core.IServices;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Core.Models;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Repositories;
using FormBuilder.Infrastructure.Repository;
using FormBuilder.Services;
using FormBuilder.Services.Mappings;
using FormBuilder.Services.Repository;
using FormBuilder.Services.Services;
using FormBuilder.Services.Services.FormBuilder;
using FormBuilder.Services.Services.FileStorage;
using FormBuilder.Services.Services.Email;
using FormBuilder.Services.Validators.FormBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace FormBuilder.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFormBuilderServices(this IServiceCollection services)
        {
            // Ensure IHttpClientFactory is available for services that call external APIs.
            services.AddHttpClient();

            // AutoMapper profiles
            services.AddAutoMapper(typeof(FormBuilderProfile).Assembly);

            // Accounts
            services.AddScoped<IaccountService, accountService>();
            services.AddScoped<IunitOfwork, UnitOfWork>();

            // Form Builder
            services.AddScoped<IFormBuilderService, FormBuilderService>();
            services.AddScoped<IFormBuilderRepository, FormBuilderRepository>();

            // Tabs
            services.AddScoped<IFormTabService, FormTabService>();
            services.AddScoped<IFormTabRepository, FormTabRepository>();

            // Fields
            services.AddScoped<IFormFieldService, FormFieldService>();
            services.AddScoped<IFormFieldRepository, FormFieldRepository>();

            // Field Types
            services.AddScoped<IFieldTypesService, FieldTypesService>();
            services.AddScoped<IFieldTypesRepository, FieldTypesRepository>();

            // Rules
            services.AddScoped<IFORM_RULESService, FORM_RULESService>();
            services.AddScoped<IFORM_RULESRepository, FORM_RULESRepository>();
            services.AddScoped<IFormRuleEvaluationService, FormRuleEvaluationService>();

            // Options
            services.AddScoped<IFieldOptionsService, FieldOptionsService>();
            services.AddScoped<IFieldOptionsRepository, FieldOptionsRepository>();

            // Data Sources
            services.AddScoped<IFieldDataSourcesService, FieldDataSourcesService>();
            services.AddScoped<IFieldDataSourcesRepository, FieldDataSourcesRepository>();

            // SAP HANA Service
            services.AddScoped<ISapHanaService, SapHanaService>();
            services.AddScoped<ISapHanaConfigsService, SapHanaConfigsService>();
            services.AddScoped<ISapDynamicIntegrationService, SapDynamicIntegrationService>();
            services.AddScoped<IHanaSecretProtector, HanaSecretProtector>();

            // Email Services (must be registered before Submissions to be injected)
            services.AddScoped<IEmailService, FormBuilder.Services.Services.Email.EmailService>();
            services.AddScoped<IEmailTemplateService, FormBuilder.Services.Services.Email.EmailTemplateService>();
            services.AddScoped<FormBuilder.Services.Services.Email.EmailNotificationService>();
            services.AddScoped<ISecretProtector, SecretProtector>();
            services.AddScoped<ISmtpConfigsService, SmtpConfigsService>();
            services.AddScoped<IEmailTemplatesService, EmailTemplatesService>();

            // Submissions
            services.AddScoped<IFormSubmissionsService, FormSubmissionsService>();
            services.AddScoped<IFormSubmissionsRepository, FormSubmissionsRepository>();

            // Attachments
            services.AddScoped<IAttachmentTypeService, AttachmentTypeService>();
            services.AddScoped<IAttachmentTypeRepository, AttachmentTypeRepository>();

            services.AddScoped<IFormAttachmentTypeService, FormAttachmentTypeService>();
            services.AddScoped<IFormAttachmentTypeRepository, FormAttachmentTypeRepository>();

            // Documents
            services.AddScoped<IDocumentTypeService, DocumentTypeService>();
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();

            services.AddScoped<IDocumentSeriesService, DocumentSeriesService>();
            services.AddScoped<IDocumentSeriesRepository, DocumentSeriesRepository>();
            services.AddScoped<IDocumentNumberGeneratorService, DocumentNumberGeneratorService>();

            // Form Builder Document Settings
            services.AddScoped<IFormBuilderDocumentSettingsService, FormBuilderDocumentSettingsService>();

            // Projects
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            // Submission Values
            services.AddScoped<IFormSubmissionValuesService, FormSubmissionValuesService>();
            services.AddScoped<IFormSubmissionValuesRepository, FormSubmissionValuesRepository>();

            // Submission Attachments
            services.AddScoped<IFormSubmissionAttachmentsService, FormSubmissionAttachmentsService>();
            services.AddScoped<IFormSubmissionAttachmentsRepository, FormSubmissionAttachmentsRepository>();

            // Grid
            services.AddScoped<IFormGridService, FormGridService>();
            services.AddScoped<IFormGridRepository, FormGridRepository>();

            services.AddScoped<IFormGridColumnService, FormGridColumnService>();
            services.AddScoped<IFormGridColumnRepository, FormGridColumnRepository>();

            services.AddScoped<IGridColumnDataSourcesService, GridColumnDataSourcesService>();
            services.AddScoped<IGridColumnDataSourcesRepository, GridColumnDataSourcesRepository>();

            services.AddScoped<IGridColumnOptionsService, GridColumnOptionsService>();
            services.AddScoped<IGridColumnOptionsRepository, GridColumnOptionsRepository>();

            services.AddScoped<IFormSubmissionGridRowService, FormSubmissionGridRowService>();
            services.AddScoped<IFormSubmissionGridRowRepository, FormSubmissionGridRowRepository>();

            services.AddScoped<IFormSubmissionGridCellService, FormSubmissionGridCellService>();
            services.AddScoped<IFormSubmissionGridCellRepository, FormSubmissionGridCellRepository>();

            // Formula
            services.AddScoped<IFormulaService, FormulaService>();
            services.AddScoped<IFormulasRepository, FormulasRepository>();
            services.AddScoped<IFormulaVariableService, FormulaVariableService>();

            // Roles
            services.AddScoped<IRoleService, RoleService>();

            // permissions
           services.AddScoped<IUserPermissionService, UserPermissionService>();
            // Approval Workflow
            services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
            services.AddScoped<IApprovalWorkflowRepository, ApprovalWorkflowRepository>();
            services.AddScoped<IApprovalStageRepository, ApprovalStageRepository>();
            services.AddScoped<IApprovalStageService, ApprovalStageService>();
            
            // Approval Stage Assignees
            services.AddScoped<IApprovalStageAssigneesRepository, ApprovalStageAssigneesRepository>();
            services.AddScoped<IApprovalStageAssigneesService, ApprovalStageAssigneesService>();
            
            // Approval Delegations
            services.AddScoped<IApprovalDelegationRepository, ApprovalDelegationRepository>();
            services.AddScoped<IApprovalDelegationService, ApprovalDelegationService>();
            
            // Document Approval History
            services.AddScoped<IDocumentApprovalHistoryRepository, DocumentApprovalHistoryRepository>();
            services.AddScoped<IDocumentApprovalHistoryService, DocumentApprovalHistoryService>();
            
            // Document Signatures
            
       
            
            // Adobe Sign OAuth Service (optional - for OAuth flow)
            
            // Form Submission Triggers (registered first to break circular dependency)
            services.AddScoped<IFormSubmissionTriggersService, FormSubmissionTriggersService>();

            // Approval Workflow Runtime (registered after triggers to break circular dependency)
            services.AddScoped<IApprovalWorkflowRuntimeService, ApprovalWorkflowRuntimeService>();
            services.AddScoped<IDocuSignAuthService, DocuSignAuthService>();
            services.AddScoped<IDocuSignEnvelopeService, DocuSignEnvelopeService>();
            services.AddScoped<ISubmitSignatureFlowService, SubmitSignatureFlowService>();
            services.AddScoped<IDocuSignService, DocuSignService>();
            services.AddScoped<ICrystalReportProxyService, CrystalReportProxyService>();

            // File Storage
            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            // Table Menus
            services.AddScoped<ITableMenusService, TableMenusService>();
            services.AddScoped<ITableMenusRepository, TableMenusRepository>();
            services.AddScoped<ITableSubMenusRepository, TableSubMenusRepository>();
            services.AddScoped<ITableMenuDocumentsRepository, TableMenuDocumentsRepository>();

            // User Queries
            services.AddScoped<IUserQueriesService, UserQueriesService>();
            services.AddScoped<IUserQueriesRepository, UserQueriesRepository>();

            // Alert Rules
            services.AddScoped<IAlertRuleService, AlertRuleService>();
            services.AddScoped<IAlertRuleRepository, AlertRuleRepository>();

            // Stored Procedures (Whitelist)
            services.AddScoped<IFormStoredProceduresService, FormStoredProceduresService>();
            services.AddScoped<IFormStoredProceduresRepository, FormStoredProceduresRepository>();

            // Stored Procedures for Form Rules
            services.AddScoped<StoredProcedureService>();

            // CopyToDocument Services
            services.AddScoped<ICopyToDocumentService, CopyToDocumentService>();
            services.AddScoped<CopyToDocumentActionExecutorService>();
            
            // IServiceScopeFactory is automatically registered by ASP.NET Core,
            // but we ensure it's available for CopyToDocumentService audit logging
            // Note: IServiceScopeFactory is a singleton by default in ASP.NET Core

            return services;
        }
    }
}
