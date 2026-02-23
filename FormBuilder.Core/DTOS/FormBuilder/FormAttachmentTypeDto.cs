namespace FormBuilder.API.Models.DTOs
{
    public class FormAttachmentTypeDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public string FormBuilderName { get; set; }
        public string AttachmentTypeName { get; set; }
        public string AttachmentTypeCode { get; set; }
        public int AttachmentTypeMaxSizeMB { get; set; }
    }

    public class CreateFormAttachmentTypeDto
    {
        public int FormBuilderId { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateFormAttachmentTypeDto
    {
        public bool? IsMandatory { get; set; }
        public bool? IsActive { get; set; }
    }

    public class toggleActiveDto
    {
        public bool IsActive { get; set; }
    }

    public class ToggleMandatoryDto
    {
        public bool IsMandatory { get; set; }
    }

    public class FormAttachmentTypeBulkDto
    {
        public int FormBuilderId { get; set; }
        public List<FormAttachmentTypeItemDto> AttachmentTypes { get; set; }
    }

    public class FormAttachmentTypeItemDto
    {
        public int AttachmentTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; } = true;
    }
}