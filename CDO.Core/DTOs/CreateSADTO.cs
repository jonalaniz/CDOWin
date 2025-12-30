namespace CDO.Core.DTOs;

public class CreateSADTO {
    // Required creation fields
    public required string id { get; init; }
    public required int clientID { get; init; }
    public required string description { get; init; }
    public required DateTime startDate { get; init; }
    public required DateTime endDate { get; init; }

    // Optional fields
    public string? office { get; set; }
    public int? counselorrID { get; set; }
    public double? unitCost { get; set; }
    public string? unitOfMeasurement { get; set; }
}