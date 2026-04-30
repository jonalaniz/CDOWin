namespace CDO.Core.DTOs.Clients.Notes;

public class NewNote {
    public required DateTime Date { get; init; }
    public required string Note { get; init; }
    public string? Author { get; init; }
}
