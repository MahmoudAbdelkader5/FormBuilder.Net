namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class AttachmentTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxSizeMB { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAttachmentTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxSizeMB { get; set; } = 10; // Default 10MB
        public bool IsActive { get; set; } = true;
    }

    public class UpdateAttachmentTypeDto
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? MaxSizeMB { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ToggleActiveDto
    {
        public bool IsActive { get; set; }
    }
}