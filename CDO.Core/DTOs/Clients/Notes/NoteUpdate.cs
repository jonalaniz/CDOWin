namespace CDO.Core.DTOs.Clients.Notes;

public record class NoteUpdate {
    public DateTime? Date { get; set; }
    public string? Note { get; set; }
    public string? Author { get; set; }
}