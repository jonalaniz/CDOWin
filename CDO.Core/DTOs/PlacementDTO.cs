namespace CDO.Core.DTOs;

public class PlacementDTO {
    public int? PlacementNumber { get; set; }
    public string? EmployerID { get; set; }
    public int? ClientID { get; set; }
    public int? CounselorID { get; set; }
    public string? PoNumber { get; set; }
    public string? Supervisor { get; set; }
    public string? SupervisorEmail { get; set; }
    public string? SupervisorPhone { get; set; }
    public string? Position { get; set; }
    public string? Salary { get; set; }
    public float? DaysOnJob { get; set; }
    public string? ClientName { get; set; }
    public string? CounselorName { get; set; }
    public bool? Active { get; set; }
    public string? Website { get; set; }
    public string? DescriptionOfDuties { get; set; }
    public string? NumbersOfHoursWorking { get; set; }
    public string? FirstFiveDays1 { get; set; }
    public string? FirstFiveDays2 { get; set; }
    public string? FirstFiveDays3 { get; set; }
    public string? FirstFiveDays4 { get; set; }
    public string? FirstFiveDays5 { get; set; }
    public string? DescriptionOfWorkSchedule { get; set; }
    public string? HourlyOrMonthlyWages { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
}