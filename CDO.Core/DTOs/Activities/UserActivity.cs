namespace CDO.Core.DTOs.Activities;

public record UserActivity(
    DateTime Date,
    string ClientName,
    string ClientID,
    string Action
    );