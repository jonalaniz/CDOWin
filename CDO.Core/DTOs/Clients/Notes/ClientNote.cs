namespace CDO.Core.DTOs.Clients.Notes;

public class ClientNote() {
    // Non-optional fields
    required public int Id { get; init; }
    required public int ClientId { get; init; }
    required public DateTime Date { get; init; }
    required public string Note { get; init; }
    public string? Author { get; init; }

    public string FormattedDate => $"{Date.ToShortDateString()} {Date.ToShortTimeString()}";
}
