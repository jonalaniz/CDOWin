namespace CDO.Core.DTOs.Clients.Notes;

public record class ClientNote(
    // Non-optional fields
    int Id,
    int ClientId,
    DateTime Date,
    string Note,
    string? Author
) {
    public string FormattedDate => $"{Date.ToLocalTime().ToString("MM/dd/yyyy hh:mm tt")}";

    public FormattedNote FormattedNote => new(FormattedDate, Note);
}

public record class FormattedNote(string Date, string Note);