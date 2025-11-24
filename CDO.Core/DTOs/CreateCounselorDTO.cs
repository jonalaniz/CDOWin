namespace CDO.Core.DTOs;

public class CreateCounselorDTO {
    // Required creation fields
    public required string name { get; init; }

    // Optional fields
    public string? email { get; init; }
    public string? phone { get; init; }
    public string? fax { get; init; }
    public string? notes { get; init; }
    public string? secretaryName { get; init; }
    public string? secretaryEmail { get; init; }
}