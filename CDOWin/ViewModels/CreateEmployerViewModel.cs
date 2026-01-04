using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDOWin.Views.Employers.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class CreateEmployerViewModel(IEmployerService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IEmployerService _service = service;

    // =========================
    // Fields
    // =========================
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public string? Supervisor { get; set; }
    public string? SupervisorPhone { get; set; }
    public string? SupervisorEmail { get; set; }


    // =========================
    // Input Validation
    // =========================
    public bool CanSave => !string.IsNullOrWhiteSpace(Name);

    // =========================
    // Property Change Methods
    // =========================
    partial void OnNameChanged(string value) {
        OnPropertyChanged(nameof(CanSave));
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task CreateEmployerAsync() {
        var employer = new EmployerDTO {
            Name = Name,
            Address1 = Address1,
            Address2 = Address2,
            City = City,
            State = State,
            Zip = Zip,
            Phone = Phone,
            Fax = Fax,
            Email = Email,
            Notes = Notes,
            Supervisor = Supervisor,
            SupervisorPhone = SupervisorPhone,
            SupervisorEmail = SupervisorEmail
        };

        await _service.CreateEmployerAsync(employer);
    }

    public void UpdateField(Field field, string value) {
        switch (field) {
            case Field.Name:
                Name = value;
                break;
            case Field.Address1:
                Address1 = value;
                break;
            case Field.Address2:
                Address2 = value;
                break;
            case Field.City:
                City = value;
                break;
            case Field.State:
                State = value;
                break;
            case Field.Zip:
                Zip = value;
                break;
            case Field.Phone:
                Phone = value;
                break;
            case Field.Fax:
                Fax = value;
                break;
            case Field.Email:
                Email = value;
                break;
            case Field.Notes:
                Notes = value;
                break;
            case Field.Supervisor:
                Supervisor = value;
                break;
            case Field.SupervisorPhone:
                SupervisorPhone = value;
                break;
            case Field.SupervisorEmail:
                SupervisorEmail = value;
                break;
        }
    }
}
