namespace CDO.Core.DTOs.Activities;

public record class ClientActivity(
    string Id,
    DateTime Date,
    string UserName,
    string UserID,
    string Action
    ) {
    public string FormattedDate => Date.ToLocalTime().ToString();
}