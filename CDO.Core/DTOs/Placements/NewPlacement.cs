namespace CDO.Core.DTOs.Placements;

public class NewPlacement {
    // Placement Specific
    public bool? Active { get; init; }
    public int? PlacementNumber { get; init; }
    public string? Position { get; init; }
    public DateTime? HireDate { get; init; }
    public DateTime? EndDate { get; init; }
    public float? DaysOnJob { get; init; }
    public string? Day1 { get; init; }
    public string? Day2 { get; init; }
    public string? Day3 { get; init; }
    public string? Day4 { get; init; }
    public string? Day5 { get; init; }
    public string? JobDuties { get; init; }
    public string? HoursWorking { get; init; }
    public string? WorkSchedule { get; init; }
    public string? Wages { get; init; }
    public string? Benefits { get; init; }

    // SA/InvoiceDetail Specific
    public int? InvoiceID { get; init; }
    public string? SaNumber { get; init; }

    // ClientDetail Specific
    public int? ClientID { get; init; }
    public string? ClientName { get; init; }

    // Counselor Specific
    public int? CounselorID { get; init; }
    public string? CounselorName { get; init; }

    // Employer Specific
    public PlacementEmployer Employer { get; init; } = default!;

}

public class PlacementEmployer {
    public int? EmployerID { get; init; }
    public string? Name { get; init; }
    public string? Phone { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public string? Fax { get; init; }
    public string? Email { get; init; }
    public string? Notes { get; init; }
    public string? SupervisorName { get; init; }
    public string? SupervisorEmail { get; init; }
    public string? SupervisorPhone { get; init; }
    public string? Website { get; init; }
}