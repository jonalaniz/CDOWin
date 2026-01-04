namespace CDO.Core.Models;

public class Reminder {
    // Required creation fields
    public int id { get; set; }
    public DateTime date { get; set; }
    public string description { get; set; }

    // Optional fields
    public int clientID { get; set; }
    public string? clientName { get; set; }
    public bool complete { get; set; }

    public string localDate => date.ToString(format: "MM/dd/yyyy");
}