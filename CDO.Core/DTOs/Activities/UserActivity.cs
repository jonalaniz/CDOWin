namespace CDO.Core.DTOs.Activities;

public record UserActivity(
    DateTime Date,
    string ClientName,
    int ClientID,
    string Action
    ) {
    public string FormattedDate => Date.ToLocalTime().ToString();
}