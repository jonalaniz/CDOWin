namespace CDO.Core.DTOs;

public class UpdateServiceAuthorizationDTO {
    public int? clientID { get; set; }
    public string? description { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
    public string? office { get; set; }
    public int? employerID { get; set; }
    public double? unitCost { get; set; }
    public string? unitOfMeasurement { get; set; }
}