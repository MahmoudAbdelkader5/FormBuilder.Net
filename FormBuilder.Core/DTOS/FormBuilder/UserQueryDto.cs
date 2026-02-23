using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class UserQueryDto
    {
        public int Id { get; set; }
        public string QueryName { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedByUserId { get; set; }
    }

    public class CreateUserQueryDto
    {
        [Required]
        [StringLength(200)]
        public string QueryName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DatabaseName { get; set; } = string.Empty;

        [Required]
        public string Query { get; set; } = string.Empty;
    }

    public class UpdateUserQueryDto
    {
        [StringLength(200)]
        public string? QueryName { get; set; }

        [StringLength(100)]
        public string? DatabaseName { get; set; }

        public string? Query { get; set; }
    }
}

