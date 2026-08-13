namespace CDO.Core.Models;

public class Reminder {
    // Required fields
    public int Id { get; set; }
    public DateTime ActionDate { get; set; }
    public string Text { get; set; } = "";
    public int ClientID { get; set; }
    public bool Completed { get; set; }

    // Optional Fields
    public string? ClientName { get; set; }

    // Computed Properties
    public string LocalDate => ActionDate.ToString(format: "MM/dd/yyyy");
}