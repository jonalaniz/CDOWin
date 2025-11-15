namespace CDO.Core.Models; 
public record class Reminder(
     int id,
     int clientID,
     string? clientName,
     string description,
     bool complete,
     DateTime date
     ) {
    public DateTime localDate => date.ToLocalTime();
}
