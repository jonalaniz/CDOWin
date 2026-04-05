namespace CDO.Core.Models;

public class Reminder {
    // Required fields
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public int ClientID { get; set; }
    public bool Complete { get; set; }

    // Optional Fields
    public string? ClientName { get; set; }

    // Computed Properties
    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
}