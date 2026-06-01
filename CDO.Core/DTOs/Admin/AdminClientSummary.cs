using System;
using System.Collections.Generic;
using System.Text;

namespace CDO.Core.DTOs.Admin; 
public class AdminClientSummary {
    // Non-optional fields
    public int Id { get; init; }
    public required bool Active { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required bool Ttw { get; init; }

    // Nullable fields
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? StartDate { get; init; }
    public string? CaseID { get; init; }
}
