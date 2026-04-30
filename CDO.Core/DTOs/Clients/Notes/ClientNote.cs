namespace CDO.Core.DTOs.Clients.Notes;

public class ClientNote() {
    // Non-optional fields
    public int Id { get; init; }
    public int ClientId { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; }
    public string Author { get; init; }
}
