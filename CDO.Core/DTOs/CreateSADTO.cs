namespace CDO.Core.DTOs;

public class CreateSADTO {
    // Required creation fields
    public required string Id { get; init; }
    public required int ClientID { get; init; }
    public required string Description { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }

    // Optional fields
    public string? Office { get; set; }
    public int? CounselorrID { get; set; }
    public double? UnitCost { get; set; }
    public string? UnitOfMeasurement { get; set; }
}