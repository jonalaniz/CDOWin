namespace CDO.Core.DTOs.SAs;

public class SAUpdate {
    // SA Specific
    public string? ServiceAuthorizationNumber { get; set; }
    public string? Office { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double? UnitCost { get; set; }
    public string? UnitOfMeasurement { get; set; }

    // Client Specific
    public int? ClientID { get; set; }
    public string? ClientName { get; set; }
    public string? CaseId { get; set; }

    // Counselor Specific
    public int? CounselorID { get; set; }
    public string? CounselorName { get; set; }
    public string? SecretaryName { get; set; }
}