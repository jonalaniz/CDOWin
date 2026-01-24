namespace CDO.Core.DTOs;

public class UpdateCounselorDTO {
    public string? Name { get; set; }
    public int? CaseLoadId { get; init; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Notes { get; set; }
    public string? SecretaryName { get; set; }
    public string? SecretaryEmail { get; set; }
}