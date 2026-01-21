namespace CDO.Core.DTOs;

public class UpdateInvoiceDTO {
    public string? ServiceAuthorizationNumber { get; set; }
    public int? ClientID { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Office { get; set; }
    public int? CounselorID { get; set; }
    public double? UnitCost { get; set; }
    public string? UnitOfMeasurement { get; set; }
}