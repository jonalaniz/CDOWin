namespace CDO.Core.DTOs;

public class PlacementDTO {
    public int? placementNumber { get; set; }
    public string? employerID { get; set; }
    public int? clientID { get; set; }
    public int? counselorID { get; set; }
    public string? poNumber { get; set; }
    public string? supervisor { get; set; }
    public string? supervisorEmail { get; set; }
    public string? supervisorPhone { get; set; }
    public string? position { get; set; }
    public string? salary { get; set; }
    public float? daysOnJob { get; set; }
    public string? clientName { get; set; }
    public string? counselorName { get; set; }
    public bool? active { get; set; }
    public string? website { get; set; }
    public string? descriptionOfDuties { get; set; }
    public string? numbersOfHoursWorking { get; set; }
    public string? firstFiveDays1 { get; set; }
    public string? firstFiveDays2 { get; set; }
    public string? firstFiveDays3 { get; set; }
    public string? firstFiveDays4 { get; set; }
    public string? firstFiveDays5 { get; set; }
    public string? descriptionOfWorkSchedule { get; set; }
    public string? hourlyOrMonthlyWages { get; set; }
    public DateTime? hireDate { get; set; }
    public DateTime? endDate { get; set; }
}