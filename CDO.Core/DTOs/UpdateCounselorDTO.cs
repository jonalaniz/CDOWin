namespace CDO.Core.DTOs;

public class UpdateCounselorDTO {
    public string? Name { get; set; }
    public int? CaseLoadID { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Notes { get; set; }
    public string? SecretaryName { get; set; }
    public string? SecretaryEmail { get; set; }

    public override string ToString() {
        return CaseLoadID == null ? Name : $"{Name}, Case Load: {CaseLoadID}";
    }
}