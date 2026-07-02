using System;
using System.Collections.Generic;
using System.Text;

namespace CDO.Core.DTOs.Admin;

public class ClientHistory {
    // Non-optional fields
    public int Id { get; init; }
    public bool Active { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required bool Ttw { get; init; }

    // ADD Activity
    public required ClientActivity[] Activities { get; init; }

    // Nullable fields
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? Zip { get; init; }
    public string? CaseID { get; init; }

    // Computed Properties
    public string Name => $"{FirstName} {LastName}";
    public bool InActive => !Active;
    public string FormattedID => $"ID: {Id}";
    public string FormattedCreatedDate => CreatedAt.ToLocalTime().ToString(format: "MM/dd/yyyy") ?? "No Date On File";
    public string FormattedUpdatedDate => UpdatedAt.ToLocalTime().ToString(format: "MM/dd/yyyy") ?? "No Date On File";
    public string FormattedUpdatedAtTime() {
        if (Time(UpdatedAt) is not string time) return "No Time On File";
        return $"Updated at: {time}";
    }
    public string FormattedAddress {
        get {
            if (Address1 == null && Address2 == null)
                return "No address on file.";
            else if (Address2 == null) {
                return $"{Address1}\n{FormattedCityStateZip}";
            } else {
                return $"{Address1} {Address2}\n{FormattedCityStateZip}";
            }
        }
    }

    public string FormattedCityStateZip {
        get {
            if (Zip != null) return $"{City}, {State} {Zip}";
            else return $"{City}, {State}";
        }
    }

    private string? Time(DateTime date) {
        return date.ToLocalTime().ToString(format: "hh:mm tt");
    }

    // Convenience Methods
    public AdminClientSummary ToggleActive() {
        return new AdminClientSummary {
            Id = Id,
            Active = !Active,
            FirstName = FirstName,
            LastName = LastName,
            City = City,
            State = State,
            Ttw = Ttw,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            Address1 = Address1,
            Address2 = Address2,
            Zip = Zip,
            CaseID = CaseID
        };
    }

    public AdminClientSummary ToggleTTW() {
        return new AdminClientSummary {
            Id = Id,
            Active = Active,
            FirstName = FirstName,
            LastName = LastName,
            City = City,
            State = State,
            Ttw = !Ttw,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            Address1 = Address1,
            Address2 = Address2,
            Zip = Zip,
            CaseID = CaseID
        };
    }
}