namespace CDO.Core.Models;

public class Reminder {
    // Required creation fields
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public required string Description { get; set; }

    // Optional fields
    public int ClientID { get; set; }
    public string? ClientName { get; set; }
    public bool Complete { get; set; }

    public string LocalDate => Date.ToString(format: "MM/dd/yyyy");
}