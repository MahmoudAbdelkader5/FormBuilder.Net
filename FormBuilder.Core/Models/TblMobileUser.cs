using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMobileUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int IdPreferableLanguage { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblMobileMessageMobileUser> TblMobileMessageMobileUsers { get; set; } = new List<TblMobileMessageMobileUser>();
}
