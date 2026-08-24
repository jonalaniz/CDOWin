namespace CDO.Core.DTOs.Admin;

public record class AdminClientNote(
    int Id,
    string ClientName,
    int ClientId,
    DateTime Date,
    string Text,
    string? Author,
    DateTime UpdatedAt
) {
    public string FormattedUpdatedTime => $"Updated at {UpdatedTime}";
    public string FormattedUpdatedOnDate => $"Updated on {UpdatedDate}";
    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
    private string UpdatedTime => UpdatedAt.ToLocalTime().ToString(format: "hh:mm tt");
    private string UpdatedDate => UpdatedAt.ToLocalTime().ToString(format: "MM/dd/yyyy hh:mm tt");
}