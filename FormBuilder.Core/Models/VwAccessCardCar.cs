using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAccessCardCar
{
    public int Id { get; set; }

    public string? PlateNumber { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? RegistrationNumber { get; set; }

    public string? Color { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public string? RegisteredIn { get; set; }

    public string? ParkingLevel { get; set; }

    public string? SpaceNumber { get; set; }

    public int? IdAccessCard { get; set; }

    public string? AccessCardCode { get; set; }

    public int? IdCarState { get; set; }

    public string CarStateName { get; set; } = null!;

    public string? CarStateForeignName { get; set; }
}
