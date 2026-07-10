namespace CDO.Core.DTOs.Clients.Notes;

public record class NewNote(
    DateTime Date,
    string Note,
    string? Author
);
