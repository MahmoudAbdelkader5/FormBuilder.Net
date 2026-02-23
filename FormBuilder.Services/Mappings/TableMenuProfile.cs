using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class TableMenuProfile : Profile
    {
        public TableMenuProfile()
        {
            // Menu Mappings
            CreateMap<TABLE_MENUS, TableMenuDto>()
                .ForMember(dest => dest.SubMenus, opt => opt.MapFrom(src => 
                    src.SubMenus.Where(sm => sm.IsActive && !sm.IsDeleted)))
                .ForMember(dest => dest.MenuDocuments, opt => opt.MapFrom(src => 
                    src.MenuDocuments.Where(md => md.IsActive && !md.IsDeleted)));

            CreateMap<CreateTableMenuDto, TABLE_MENUS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.SubMenus, opt => opt.Ignore())
                .ForMember(dest => dest.MenuDocuments, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore());

            CreateMap<UpdateTableMenuDto, TABLE_MENUS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MenuCode, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.SubMenus, opt => opt.Ignore())
                .ForMember(dest => dest.MenuDocuments, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Sub Menu Mappings
            CreateMap<TABLE_SUB_MENUS, TableSubMenuDto>()
                .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu.Name))
                .ForMember(dest => dest.MenuDocuments, opt => opt.MapFrom(src => 
                    src.MenuDocuments.Where(md => md.IsActive && !md.IsDeleted)));

            CreateMap<CreateTableSubMenuDto, TABLE_SUB_MENUS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.MenuDocuments, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore());

            CreateMap<UpdateTableSubMenuDto, TABLE_SUB_MENUS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MenuId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.MenuDocuments, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Menu Document Mappings
            CreateMap<TABLE_MENU_DOCUMENTS, TableMenuDocumentDto>()
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.Name))
                .ForMember(dest => dest.DocumentTypeCode, opt => opt.MapFrom(src => src.DocumentType.Code))
                .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu != null ? src.Menu.Name : null))
                .ForMember(dest => dest.SubMenuName, opt => opt.MapFrom(src => src.SubMenu != null ? src.SubMenu.Name : null));

            CreateMap<CreateTableMenuDocumentDto, TABLE_MENU_DOCUMENTS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.SubMenu, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore());

            CreateMap<UpdateTableMenuDocumentDto, TABLE_MENU_DOCUMENTS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.SubMenu, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

