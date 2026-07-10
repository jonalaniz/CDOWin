namespace CDO.Core.DTOs.Counselors;

public record class CounselorSummary(
    int Id,
    string Name,
    int? CaseLoadID,
    string? Phone,
    string? Email,
    string? SecretaryName
) {
    // Computed Properties
    public string FormattedCaseLoadID => CaseLoadID == null ? "No case load ID on file." : $"Case Load: {CaseLoadID}";

    public override string ToString() {
        return CaseLoadID == null ? Name : $"{Name}, Case Load: {CaseLoadID}";
    }
}
