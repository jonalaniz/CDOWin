using CDO.Core.DTOs.Counselors;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Views.Counselors.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateCounselorViewModel(ICounselorService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly ICounselorService _service = service;

    // =========================
    // Fields
    // =========================
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSave))]
    public partial string Name { get; set; } = string.Empty;
    public int? CaseLoadId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Notes { get; set; }
    public string? SecretaryName { get; set; }
    public string? SecretaryEmail { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => !string.IsNullOrWhiteSpace(Name);

    // =========================
    // CRUD Methods
    // =========================
    public async Task<Result<Counselor>> CreateCounselorAsync() {
        var counselor = new NewCounselor {
            name = Name,
            CaseLoadID = CaseLoadId,
            Email = Email,
            Phone = Phone,
            Fax = Fax,
            Notes = Notes,
            SecretaryName = SecretaryName,
            SecretaryEmail = SecretaryEmail
        };

        return await _service.CreateCounselorAsync(counselor);
    }

    public void UpdateField(Field field, string value) {
        switch (field) {
            case Field.Name:
                Name = value;
                break;
            case Field.Email:
                Email = value;
                break;
            case Field.Phone:
                Phone = value;
                break;
            case Field.Fax:
                Fax = value;
                break;
            case Field.Notes:
                Notes = value;
                break;
            case Field.Secretary:
                SecretaryName = value;
                break;
            case Field.SecretaryEmail:
                SecretaryEmail = value;
                break;
        }
    }
}
