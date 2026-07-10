namespace CDO.Core.DTOs.Placements;

public record class NewPlacement(
    // Placement Specific
    bool Active,
    int? PlacementNumber,
    string? Position,
    DateTime? HireDate,
    DateTime? EndDate,
    int? DaysOnJob,
    string? Day1,
    string? Day2,
    string? Day3,
    string? Day4,
    string? Day5,
    string? JobDuties,
    string? WorkEnvironment,
    string? Accommodations,
    string? HoursWorking,
    string? WorkSchedule,
    string? Wages,
    string? Benefits,

    // SA/SADetail Specific
    int? SaID,
    string? SaNumber,

    // ClientDetail Specific
    int? ClientID,
    string? ClientName,

    // Counselor Specific
    int? CounselorID,
    string? CounselorName,

    // Employer Specific
    PlacementEmployer Employer = default!
);

public record class PlacementEmployer(
    int? EmployerID,
    string? Name,
    string? Phone,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Zip,
    string? Fax,
    string? Email,
    string? Notes,
    string? SupervisorName,
    string? SupervisorEmail,
    string? SupervisorPhone,
    string? Website
);