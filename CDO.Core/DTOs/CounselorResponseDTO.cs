using CDO.Core.DTOs.Clients;
using CDO.Core.Models;

namespace CDO.Core.DTOs;

public class CounselorResponseDTO {
    // Non-optional fields
    public int Id { get; init; }
    public required string Name { get; init; }
    public required ClientSummary[] Clients { get; init; }
    public required Invoice[] Invoices { get; init; }

    // Nullable fields
    public int? CaseLoadId { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Fax { get; init; }
    public string? Notes { get; init; }
    public string? SecretaryName { get; init; }
    public string? SecretaryEmail { get; init; }
}
