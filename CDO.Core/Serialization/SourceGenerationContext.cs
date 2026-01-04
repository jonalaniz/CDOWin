using CDO.Core.DTOs;
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
[JsonSerializable(typeof(List<Counselor>))]
[JsonSerializable(typeof(CreateCounselorDTO))]
[JsonSerializable(typeof(UpdateCounselorDTO))]

[JsonSerializable(typeof(Employer))]
[JsonSerializable(typeof(List<Employer>))]
[JsonSerializable(typeof(EmployerDTO))]

[JsonSerializable(typeof(Placement))]
[JsonSerializable(typeof(List<Placement>))]
[JsonSerializable(typeof(PlacementDTO))]

[JsonSerializable(typeof(Reminder))]
[JsonSerializable(typeof(List<Reminder>))]
[JsonSerializable(typeof(CreateReminderDTO))]
[JsonSerializable(typeof(UpdateReminderDTO))]

[JsonSerializable(typeof(ServiceAuthorization))]
[JsonSerializable(typeof(List<ServiceAuthorization>))]
[JsonSerializable(typeof(CreateSADTO))]
[JsonSerializable(typeof(UpdateServiceAuthorizationDTO))]

[JsonSerializable(typeof(State))]
[JsonSerializable(typeof(List<State>))]
[JsonSerializable(typeof(CreateStateDTO))]
[JsonSerializable(typeof(UpdateStateDTO))]

public partial class SourceGenerationContext : JsonSerializerContext { }
