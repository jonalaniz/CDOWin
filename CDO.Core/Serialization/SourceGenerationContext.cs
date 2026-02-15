using CDO.Core.DTOs;
using CDO.Core.DTOs.Clients;
using CDO.Core.DTOs.Counselors;
using CDO.Core.DTOs.Employers;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.Reminders;
using CDO.Core.DTOs.SAs;
using CDO.Core.Models;
using System.Text.Json.Serialization;

namespace CDO.Core.Serialization;

[JsonSerializable(typeof(ClientDetail))]
[JsonSerializable(typeof(List<ClientDetail>))]
[JsonSerializable(typeof(NewClient))]
[JsonSerializable(typeof(ClientUpdate))]

[JsonSerializable(typeof(ClientSummary))]
[JsonSerializable(typeof(List<ClientSummary>))]

[JsonSerializable(typeof(Counselor))]
[JsonSerializable(typeof(CounselorDetail))]
[JsonSerializable(typeof(List<Counselor>))]
[JsonSerializable(typeof(NewCounselor))]
[JsonSerializable(typeof(CounselorUpdate))]

[JsonSerializable(typeof(CounselorSummary))]
[JsonSerializable(typeof(List<CounselorSummary>))]

[JsonSerializable(typeof(Employer))]
[JsonSerializable(typeof(List<Employer>))]
[JsonSerializable(typeof(EmployerDTO))]
[JsonSerializable(typeof(PlacementEmployer))]

[JsonSerializable(typeof(EmployerSummary))]
[JsonSerializable(typeof(List<EmployerSummary>))]

[JsonSerializable(typeof(PlacementDetail))]
[JsonSerializable(typeof(List<PlacementDetail>))]
[JsonSerializable(typeof(NewPlacement))]
[JsonSerializable(typeof(PlacementUpdate))]

[JsonSerializable(typeof(PlacementSummary))]
[JsonSerializable(typeof(List<PlacementSummary>))]

[JsonSerializable(typeof(Reminder))]
[JsonSerializable(typeof(List<Reminder>))]
[JsonSerializable(typeof(NewReminder))]
[JsonSerializable(typeof(ReminderUpdate))]

[JsonSerializable(typeof(InvoiceDetail))]
[JsonSerializable(typeof(InvoiceSummary))]
[JsonSerializable(typeof(List<InvoiceSummary>))]
[JsonSerializable(typeof(NewSA))]
[JsonSerializable(typeof(SAUpdate))]

[JsonSerializable(typeof(State))]
[JsonSerializable(typeof(List<State>))]
[JsonSerializable(typeof(CreateStateDTO))]
[JsonSerializable(typeof(UpdateStateDTO))]

public partial class SourceGenerationContext : JsonSerializerContext { }
