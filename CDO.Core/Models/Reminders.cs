namespace CDO.Core.Models;

public record class Reminder(
     int id,
     int clientID,
     string? clientName,
     string description,
     bool complete,
     DateTime date
     ) {
    public string localDate => date.ToString(format: "MM/dd/yyyy");
}
