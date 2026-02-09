using CDO.Core.DTOs;
using CDO.Core.DTOs.Placements;
using CDO.Core.Models;
using System.Text.Json.Serialization;

namespace CDO.Core.Serialization;

[JsonSerializable(typeof(Client))]
[JsonSerializable(typeof(List<Client>))]
[JsonSerializable(typeof(CreateClientDTO))]
[JsonSerializable(typeof(UpdateClientDTO))]

[JsonSerializable(typeof(ClientSummaryDTO))]
[JsonSerializable(typeof(List<ClientSummaryDTO>))]

[JsonSerializable(typeof(Counselor))]
[JsonSerializable(typeof(CounselorResponseDTO))]
[JsonSerializable(typeof(List<Counselor>))]
[JsonSerializable(typeof(CreateCounselorDTO))]
[JsonSerializable(typeof(UpdateCounselorDTO))]

[JsonSerializable(typeof(CounselorSummaryDTO))]
[JsonSerializable(typeof(List<CounselorSummaryDTO>))]

[JsonSerializable(typeof(Employer))]
[JsonSerializable(typeof(List<Employer>))]
[JsonSerializable(typeof(EmployerDTO))]
[JsonSerializable(typeof(PlacementEmployer))]

[JsonSerializable(typeof(EmployerSummaryDTO))]
[JsonSerializable(typeof(List<EmployerSummaryDTO>))]

[JsonSerializable(typeof(PlacementDetail))]
[JsonSerializable(typeof(List<PlacementDetail>))]
[JsonSerializable(typeof(NewPlacement))]
[JsonSerializable(typeof(PlacementUpdate))]

[JsonSerializable(typeof(PlacementSummary))]
[JsonSerializable(typeof(List<PlacementSummary>))]

[JsonSerializable(typeof(Reminder))]
[JsonSerializable(typeof(List<Reminder>))]
[JsonSerializable(typeof(CreateReminderDTO))]
[JsonSerializable(typeof(UpdateReminderDTO))]

[JsonSerializable(typeof(Invoice))]
[JsonSerializable(typeof(List<Invoice>))]
[JsonSerializable(typeof(CreateInvoiceDTO))]
[JsonSerializable(typeof(UpdateInvoiceDTO))]

[JsonSerializable(typeof(State))]
[JsonSerializable(typeof(List<State>))]
[JsonSerializable(typeof(CreateStateDTO))]
[JsonSerializable(typeof(UpdateStateDTO))]

public partial class SourceGenerationContext : JsonSerializerContext { }
