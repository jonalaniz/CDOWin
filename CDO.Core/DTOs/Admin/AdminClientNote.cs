namespace CDO.Core.DTOs.Admin;

public record class AdminClientNote(
    int Id,
    string ClientName,
    int ClientId,
    DateTime Date,
    string Note,
    string? Author
);
