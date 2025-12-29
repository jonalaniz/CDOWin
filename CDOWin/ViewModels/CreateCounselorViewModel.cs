using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDOWin.Views.Counselors.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateCounselorViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly ICounselorService _service;

    // =========================
    // Fields
    // =========================
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? Email { get; set; }

    [ObservableProperty]
    public partial string? Phone { get; set; }

    [ObservableProperty]
    public partial string? Fax { get; set; }

    [ObservableProperty]
    public partial string? Notes { get; set; }

    [ObservableProperty]
    public partial string? SecretaryName { get; set; }

    [ObservableProperty]
    public partial string? SecretaryEmail { get; set; }

    // =========================
    // Input Validation
    // =========================
    public bool CanSave => !string.IsNullOrWhiteSpace(Name);

    // =========================
    // Constructor
    // =========================
    public CreateCounselorViewModel(ICounselorService service) {
        _service = service;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnNameChanged(string value) {
        OnPropertyChanged(nameof(CanSave));
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreateCounselorAsync() {
        var counselor = new CreateCounselorDTO {
            name = Name,
            email = Email,
            phone = Phone,
            fax = Fax,
            notes = Notes,
            secretaryName = SecretaryName,
            secretaryEmail = SecretaryEmail
        };

        await _service.CreateCounselorAsync(counselor);
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
