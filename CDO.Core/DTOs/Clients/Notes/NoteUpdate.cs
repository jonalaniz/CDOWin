namespace CDO.Core.DTOs.Clients.Notes;

public record class NoteUpdate {
    public DateTime? Date { get; set; }
    public string? Text { get; set; }
    public string? Author { get; set; }
}