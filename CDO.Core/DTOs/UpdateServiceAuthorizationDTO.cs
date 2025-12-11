namespace CDO.Core.DTOs;

public class UpdateServiceAuthorizationDTO {
    public int? clientID { get; init; }
    public string? description { get; init; }
    public DateTime? startDate { get; init; }
    public DateTime? endDate { get; init; }
    public string? office { get; set; }
    public int? employerID { get; set; }
    public double? unitCost { get; set; }
    public string? unitOfMeasurement { get; set; }
}