using System;
using System.Diagnostics.CodeAnalysis;

namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // ==========================
    // DTO لعرض البيانات
    // ==========================
    public class ApprovalStageAssigneesDto
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }

    // ==========================
    // DTO لإنشاء Assignee جديد
    // ==========================
    public class ApprovalStageAssigneesCreateDto
    {
        public int StageId { get; set; }
        
        /// <summary>
        /// RoleId for role-based assignment. Set to null for user-specific assignment.
        /// </summary>
        [AllowNull]
        public string RoleId { get; set; } // nullable - for role-based assignment
        
        /// <summary>
        /// UserId for user-specific assignment. Set to null for role-only assignment.
        /// </summary>
        [AllowNull]
        public string UserId { get; set; } // nullable - for user-specific assignment
        
        public bool IsActive { get; set; } = true;
    }

    // ==========================
    // DTO لتحديث Assignee موجود
    // ==========================
    public class ApprovalStageAssigneesUpdateDto
    {
        public int? StageId { get; set; }
        public string UserId { get; set; } // RoleId will be extracted from UserId automatically
        public bool? IsActive { get; set; }
    }

    // ==========================
    // DTO لإدارة Assignees لمرحلة معينة
    // ==========================
    public class StageAssigneesBulkDto
    {
        public int StageId { get; set; }
        public string[] RoleIds { get; set; } = Array.Empty<string>();
        public string[] UserIds { get; set; } = Array.Empty<string>();
    }
}

