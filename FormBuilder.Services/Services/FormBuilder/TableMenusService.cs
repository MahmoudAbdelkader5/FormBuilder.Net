using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using formBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class TableMenusService : ITableMenusService
    {
        private readonly ITableMenusRepository _menusRepository;
        private readonly ITableSubMenusRepository _subMenusRepository;
        private readonly ITableMenuDocumentsRepository _menuDocumentsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TableMenusService> _logger;
        private readonly IunitOfwork _unitOfWork;
        private readonly FormBuilderDbContext _context;

        public TableMenusService(
            ITableMenusRepository menusRepository,
            ITableSubMenusRepository subMenusRepository,
            ITableMenuDocumentsRepository menuDocumentsRepository,
            IMapper mapper,
            ILogger<TableMenusService> logger,
            IunitOfwork unitOfWork,
            FormBuilderDbContext context)
        {
            _menusRepository = menusRepository ?? throw new ArgumentNullException(nameof(menusRepository));
            _subMenusRepository = subMenusRepository ?? throw new ArgumentNullException(nameof(subMenusRepository));
            _menuDocumentsRepository = menuDocumentsRepository ?? throw new ArgumentNullException(nameof(menuDocumentsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Menu Operations

        public async Task<ApiResponse> GetAllMenusAsync()
        {
            try
            {
                var menus = await _menusRepository.GetAllActiveAsync();
                var menusDto = _mapper.Map<List<TableMenuDto>>(menus);
                return new ApiResponse(200, "Menus retrieved successfully", menusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all menus");
                return new ApiResponse(500, $"Error retrieving menus: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMenuByIdAsync(int id)
        {
            try
            {
                var menu = await _menusRepository.GetByIdWithSubMenusAsync(id);
                if (menu == null)
                    return new ApiResponse(404, "Menu not found");

                var menuDto = _mapper.Map<TableMenuDto>(menu);
                return new ApiResponse(200, "Menu retrieved successfully", menuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu {MenuId}", id);
                return new ApiResponse(500, $"Error retrieving menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMenuByCodeAsync(string menuCode)
        {
            try
            {
                var menu = await _menusRepository.GetByMenuCodeAsync(menuCode);
                if (menu == null)
                    return new ApiResponse(404, "Menu not found");

                var menuDto = _mapper.Map<TableMenuDto>(menu);
                return new ApiResponse(200, "Menu retrieved successfully", menuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu by code {MenuCode}", menuCode);
                return new ApiResponse(500, $"Error retrieving menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateMenuAsync(CreateTableMenuDto createDto)
        {
            try
            {
                // Check if menu code already exists
                var existingMenu = await _menusRepository.GetByMenuCodeAsync(createDto.MenuCode);
                if (existingMenu != null && !existingMenu.IsDeleted)
                    return new ApiResponse(400, $"Menu with code '{createDto.MenuCode}' already exists");

                var menu = _mapper.Map<TABLE_MENUS>(createDto);
                menu.CreatedDate = DateTime.UtcNow;

                _menusRepository.Add(menu);
                await _unitOfWork.CompleteAsyn();


                var menuDto = _mapper.Map<TableMenuDto>(menu);
                return new ApiResponse(201, "Menu created successfully", menuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu");
                return new ApiResponse(500, $"Error creating menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateMenuAsync(int id, UpdateTableMenuDto updateDto)
        {
            try
            {
                var menu = await _menusRepository.SingleOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
                if (menu == null)
                    return new ApiResponse(404, "Menu not found");

                _mapper.Map(updateDto, menu);
                menu.UpdatedDate = DateTime.UtcNow;

                _menusRepository.Update(menu);
                await _unitOfWork.CompleteAsyn();

                var menuDto = _mapper.Map<TableMenuDto>(menu);
                return new ApiResponse(200, "Menu updated successfully", menuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu {MenuId}", id);
                return new ApiResponse(500, $"Error updating menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _menusRepository.SingleOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
                if (menu == null)
                    return new ApiResponse(404, "Menu not found");

                _menusRepository.Delete(menu);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Menu deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu {MenuId}", id);
                return new ApiResponse(500, $"Error deleting menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SoftDeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _menusRepository.SingleOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
                if (menu == null)
                    return new ApiResponse(404, "Menu not found");

                menu.IsDeleted = true;
                menu.DeletedDate = DateTime.UtcNow;
                menu.IsActive = false;

                _menusRepository.Update(menu);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Menu soft deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting menu {MenuId}", id);
                return new ApiResponse(500, $"Error soft deleting menu: {ex.Message}");
            }
        }

        #endregion

        #region Sub Menu Operations

        public async Task<ApiResponse> GetAllSubMenusAsync()
        {
            try
            {
                var subMenus = await _subMenusRepository.GetAllAsync();
                var subMenusDto = _mapper.Map<List<TableSubMenuDto>>(subMenus);
                return new ApiResponse(200, "Sub menus retrieved successfully", subMenusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all sub menus");
                return new ApiResponse(500, $"Error retrieving sub menus: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetSubMenusByMenuIdAsync(int menuId)
        {
            try
            {
                var subMenus = await _subMenusRepository.GetActiveByMenuIdAsync(menuId);
                var subMenusDto = _mapper.Map<List<TableSubMenuDto>>(subMenus);
                return new ApiResponse(200, "Sub menus retrieved successfully", subMenusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sub menus for menu {MenuId}", menuId);
                return new ApiResponse(500, $"Error retrieving sub menus: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetSubMenuByIdAsync(int id)
        {
            try
            {
                var subMenu = await _subMenusRepository.GetByIdWithDocumentsAsync(id);
                if (subMenu == null)
                    return new ApiResponse(404, "Sub menu not found");

                var subMenuDto = _mapper.Map<TableSubMenuDto>(subMenu);
                return new ApiResponse(200, "Sub menu retrieved successfully", subMenuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sub menu {SubMenuId}", id);
                return new ApiResponse(500, $"Error retrieving sub menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateSubMenuAsync(CreateTableSubMenuDto createDto)
        {
            try
            {
                // Verify parent menu exists
                var menu = await _menusRepository.SingleOrDefaultAsync(m => m.Id == createDto.MenuId && !m.IsDeleted);
                if (menu == null)
                    return new ApiResponse(404, "Parent menu not found");

                var subMenu = _mapper.Map<TABLE_SUB_MENUS>(createDto);
                subMenu.CreatedDate = DateTime.UtcNow;

                _subMenusRepository.Add(subMenu);
                await _unitOfWork.CompleteAsyn();

                var subMenuDto = _mapper.Map<TableSubMenuDto>(subMenu);
                return new ApiResponse(201, "Sub menu created successfully", subMenuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sub menu");
                return new ApiResponse(500, $"Error creating sub menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateSubMenuAsync(int id, UpdateTableSubMenuDto updateDto)
        {
            try
            {
                var subMenu = await _subMenusRepository.SingleOrDefaultAsync(sm => sm.Id == id && !sm.IsDeleted);
                if (subMenu == null)
                    return new ApiResponse(404, "Sub menu not found");

                _mapper.Map(updateDto, subMenu);
                subMenu.UpdatedDate = DateTime.UtcNow;

                _subMenusRepository.Update(subMenu);
                await _unitOfWork.CompleteAsyn();

                var subMenuDto = _mapper.Map<TableSubMenuDto>(subMenu);
                return new ApiResponse(200, "Sub menu updated successfully", subMenuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sub menu {SubMenuId}", id);
                return new ApiResponse(500, $"Error updating sub menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteSubMenuAsync(int id)
        {
            try
            {
                var subMenu = await _subMenusRepository.SingleOrDefaultAsync(sm => sm.Id == id && !sm.IsDeleted);
                if (subMenu == null)
                    return new ApiResponse(404, "Sub menu not found");

                _subMenusRepository.Delete(subMenu);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Sub menu deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sub menu {SubMenuId}", id);
                return new ApiResponse(500, $"Error deleting sub menu: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SoftDeleteSubMenuAsync(int id)
        {
            try
            {
                var subMenu = await _subMenusRepository.SingleOrDefaultAsync(sm => sm.Id == id && !sm.IsDeleted);
                if (subMenu == null)
                    return new ApiResponse(404, "Sub menu not found");

                subMenu.IsDeleted = true;
                subMenu.DeletedDate = DateTime.UtcNow;
                subMenu.IsActive = false;

                _subMenusRepository.Update(subMenu);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Sub menu soft deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting sub menu {SubMenuId}", id);
                return new ApiResponse(500, $"Error soft deleting sub menu: {ex.Message}");
            }
        }

        #endregion

        #region Menu Document Operations

        public async Task<ApiResponse> GetAllMenuDocumentsAsync()
        {
            try
            {
                var documents = await _menuDocumentsRepository.GetAllAsync();
                var documentsDto = _mapper.Map<List<TableMenuDocumentDto>>(documents);
                return new ApiResponse(200, "Menu documents retrieved successfully", documentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all menu documents");
                return new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMenuDocumentsByMenuIdAsync(int menuId)
        {
            try
            {
                var documents = await _menuDocumentsRepository.GetActiveByMenuIdAsync(menuId);
                var documentsDto = _mapper.Map<List<TableMenuDocumentDto>>(documents);
                return new ApiResponse(200, "Menu documents retrieved successfully", documentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu documents for menu {MenuId}", menuId);
                return new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMenuDocumentsBySubMenuIdAsync(int subMenuId)
        {
            try
            {
                var documents = await _menuDocumentsRepository.GetActiveBySubMenuIdAsync(subMenuId);
                var documentsDto = _mapper.Map<List<TableMenuDocumentDto>>(documents);
                return new ApiResponse(200, "Menu documents retrieved successfully", documentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu documents for sub menu {SubMenuId}", subMenuId);
                return new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMenuDocumentByIdAsync(int id)
        {
            try
            {
                var document = await _menuDocumentsRepository.SingleOrDefaultAsync(md => md.Id == id && !md.IsDeleted);
                if (document == null)
                    return new ApiResponse(404, "Menu document not found");

                var documentDto = _mapper.Map<TableMenuDocumentDto>(document);
                return new ApiResponse(200, "Menu document retrieved successfully", documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu document {MenuDocumentId}", id);
                return new ApiResponse(500, $"Error retrieving menu document: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateMenuDocumentAsync(CreateTableMenuDocumentDto createDto)
        {
            try
            {
                // Validate that either MenuId or SubMenuId is provided
                if (!createDto.MenuId.HasValue && !createDto.SubMenuId.HasValue)
                    return new ApiResponse(400, "Either MenuId or SubMenuId must be provided");

                // If SubMenuId is provided, verify it exists and belongs to MenuId if MenuId is also provided
                if (createDto.SubMenuId.HasValue)
                {
                    var subMenu = await _subMenusRepository.SingleOrDefaultAsync(sm => sm.Id == createDto.SubMenuId.Value && !sm.IsDeleted);
                    if (subMenu == null)
                        return new ApiResponse(404, "Sub menu not found");

                    if (createDto.MenuId.HasValue && subMenu.MenuId != createDto.MenuId.Value)
                        return new ApiResponse(400, "Sub menu does not belong to the specified menu");
                }
                else if (createDto.MenuId.HasValue)
                {
                    var menu = await _menusRepository.SingleOrDefaultAsync(m => m.Id == createDto.MenuId.Value && !m.IsDeleted);
                    if (menu == null)
                        return new ApiResponse(404, "Menu not found");
                }

                // Verify document type exists
                // This would require IDocumentTypeRepository - we'll assume it exists for now

                var menuDocument = _mapper.Map<TABLE_MENU_DOCUMENTS>(createDto);
                menuDocument.CreatedDate = DateTime.UtcNow;

                _menuDocumentsRepository.Add(menuDocument);
                await _unitOfWork.CompleteAsyn();

                var documentDto = _mapper.Map<TableMenuDocumentDto>(menuDocument);
                return new ApiResponse(201, "Menu document created successfully", documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu document");
                return new ApiResponse(500, $"Error creating menu document: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateMenuDocumentAsync(int id, UpdateTableMenuDocumentDto updateDto)
        {
            try
            {
                var menuDocument = await _menuDocumentsRepository.SingleOrDefaultAsync(md => md.Id == id && !md.IsDeleted);
                if (menuDocument == null)
                    return new ApiResponse(404, "Menu document not found");

                _mapper.Map(updateDto, menuDocument);
                menuDocument.UpdatedDate = DateTime.UtcNow;

                _menuDocumentsRepository.Update(menuDocument);
                await _unitOfWork.CompleteAsyn();

                var documentDto = _mapper.Map<TableMenuDocumentDto>(menuDocument);
                return new ApiResponse(200, "Menu document updated successfully", documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu document {MenuDocumentId}", id);
                return new ApiResponse(500, $"Error updating menu document: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteMenuDocumentAsync(int id)
        {
            try
            {
                var menuDocument = await _menuDocumentsRepository.SingleOrDefaultAsync(md => md.Id == id && !md.IsDeleted);
                if (menuDocument == null)
                    return new ApiResponse(404, "Menu document not found");

                _menuDocumentsRepository.Delete(menuDocument);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Menu document deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu document {MenuDocumentId}", id);
                return new ApiResponse(500, $"Error deleting menu document: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SoftDeleteMenuDocumentAsync(int id)
        {
            try
            {
                var menuDocument = await _menuDocumentsRepository.SingleOrDefaultAsync(md => md.Id == id && !md.IsDeleted);
                if (menuDocument == null)
                    return new ApiResponse(404, "Menu document not found");

                menuDocument.IsDeleted = true;
                menuDocument.DeletedDate = DateTime.UtcNow;
                menuDocument.IsActive = false;

                _menuDocumentsRepository.Update(menuDocument);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Menu document soft deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting menu document {MenuDocumentId}", id);
                return new ApiResponse(500, $"Error soft deleting menu document: {ex.Message}");
            }
        }

        #endregion

        #region Dashboard Operations

        public async Task<ApiResponse> GetDashboardMenusAsync(List<string>? userPermissions = null)
        {
            try
            {
                var allMenus = await _menusRepository.GetAllActiveAsync();
                var dashboardMenus = new List<DashboardMenuDto>();

                foreach (var menu in allMenus)
                {

                    var dashboardMenu = new DashboardMenuDto
                    {
                        Id = menu.Id,
                        Name = menu.Name,
                        ForeignName = menu.ForeignName,
                        MenuCode = menu.MenuCode
                    };

                    // Get sub menus
                    var subMenus = await _subMenusRepository.GetActiveByMenuIdAsync(menu.Id);
                    var dashboardSubMenus = new List<DashboardSubMenuDto>();

                    foreach (var subMenu in subMenus)
                    {
                        var dashboardSubMenu = new DashboardSubMenuDto
                        {
                            Id = subMenu.Id,
                            Name = subMenu.Name,
                            ForeignName = subMenu.ForeignName
                        };

                        // Get documents for this sub menu
                        var subMenuDocuments = await _menuDocumentsRepository.GetActiveBySubMenuIdAsync(subMenu.Id);
                        dashboardSubMenu.Documents = subMenuDocuments
                            .Select(md => new DashboardDocumentDto
                            {
                                Id = md.Id,
                                DocumentTypeId = md.DocumentTypeId,
                                DocumentTypeName = md.DocumentType?.Name ?? "",
                                DocumentTypeCode = md.DocumentType?.Code ?? ""
                            })
                            .ToList();

                        if (dashboardSubMenu.Documents.Any())
                            dashboardSubMenus.Add(dashboardSubMenu);
                    }

                    dashboardMenu.SubMenus = dashboardSubMenus.ToList();

                    // Get direct documents (not in sub menu)
                    var directDocuments = await _menuDocumentsRepository.GetActiveByMenuIdAsync(menu.Id);
                    dashboardMenu.Documents = directDocuments
                        .Select(md => new DashboardDocumentDto
                        {
                            Id = md.Id,
                            DocumentTypeId = md.DocumentTypeId,
                            DocumentTypeName = md.DocumentType?.Name ?? "",
                            DocumentTypeCode = md.DocumentType?.Code ?? ""
                        })
                        .ToList();

                    // Only add menu if it has sub menus or documents
                    if (dashboardMenu.SubMenus.Any() || dashboardMenu.Documents.Any())
                        dashboardMenus.Add(dashboardMenu);
                }

                return new ApiResponse(200, "Dashboard menus retrieved successfully", dashboardMenus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard menus");
                return new ApiResponse(500, $"Error retrieving dashboard menus: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetDashboardMenuByIdAsync(int menuId, List<string>? userPermissions = null)
        {
            try
            {
                var menu = await _menusRepository.GetByIdWithSubMenusAsync(menuId);
                if (menu == null || menu.IsDeleted || !menu.IsActive)
                    return new ApiResponse(404, "Menu not found");

                var dashboardMenu = new DashboardMenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    ForeignName = menu.ForeignName,
                    MenuCode = menu.MenuCode
                };

                // Get sub menus and documents similar to GetDashboardMenusAsync
                // (Implementation would be similar, just for one menu)

                return new ApiResponse(200, "Dashboard menu retrieved successfully", dashboardMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard menu {MenuId}", menuId);
                return new ApiResponse(500, $"Error retrieving dashboard menu: {ex.Message}");
            }
        }

        #endregion
    }
}

