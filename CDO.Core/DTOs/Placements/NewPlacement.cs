namespace CDO.Core.DTOs.Placements;

public class NewPlacement {
    // Placement Specific
    public bool? Active { get; set; }
    public int? PlacementNumber { get; set; }
    public string? Position { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float? DaysOnJob { get; set; }
    public string? Day1 { get; set; }
    public string? Day2 { get; set; }
    public string? Day3 { get; set; }
    public string? Day4 { get; set; }
    public string? Day5 { get; set; }
    public string? JobDuties { get; set; }
    public string? HoursWorking { get; set; }
    public string? WorkSchedule { get; set; }
    public string? Wages { get; set; }
    public string? Benefits { get; set; }

    // SA/Invoice Specific
    public int? InvoiceID { get; set; }
    public string? SaNumber { get; set; }

    // Client Specific
    public int? ClientID { get; set; }
    public string? ClientName { get; set; }

    // Counselor Specific
    public int? CounselorID { get; set; }
    public string? CounselorName { get; set; }

    // Employer Specific
    public string? EmployerID { get; set; }
    public string? EmployerName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? SupervisorName { get; set; }
    public string? SupervisorEmail { get; set; }
    public string? SupervisorPhone { get; set; }
    public string? Website { get; set; }
}
