namespace CDOWin.Views.Placements.Dialogs;

public enum UpdateField {
    // Placement Specific
    Active,
    PlacementNumber,
    Position,
    HireDate,
    EndDate,
    DaysOnJob,
    Day1,
    Day2,
    Day3,
    Day4,
    Day5,
    JobDuties,
    HoursWorking,
    WorkSchedule,
    Wage,
    Benefits,

    // SA/Invoice Specific
    InvoiceID,
    SANumber,

    // Client Specific
    ClientID,
    ClientName,

    // Counselor Specific
    CounselorID,
    CounselorName,

    // Employer Specific
    EmployerID,
    EmployerName,
    EmployerPhone,
    Address1,
    Address2,
    City,
    State,
    Zip,
    SupervisorName,
    SupervisorEmail,
    SupervisorPhone,
    Website
}