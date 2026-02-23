using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGlaccountDeterminationDetail
{
    public int Id { get; set; }

    public int IdGlaccountDetermination { get; set; }

    public int? IdGlaccount { get; set; }

    public int? IdTransactionType { get; set; }

    public int? IdProperty { get; set; }

    public int? IdParticular { get; set; }

    public int? IdState { get; set; }

    public int? IdCancellationType { get; set; }

    public int? IdCancellationTypeChecklist { get; set; }

    public int? IdUser { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdCustomer { get; set; }

    public virtual TblCancellationTypeChecklist? IdCancellationTypeChecklistNavigation { get; set; }

    public virtual TblCancellationType? IdCancellationTypeNavigation { get; set; }

    public virtual TblCustomer? IdCustomerNavigation { get; set; }

    public virtual TblGlaccountDetermination IdGlaccountDeterminationNavigation { get; set; } = null!;

    public virtual TblGlaccount? IdGlaccountNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParticular? IdParticularNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }

    public virtual TblState? IdStateNavigation { get; set; }

    public virtual TblUser? IdUserNavigation { get; set; }
}
