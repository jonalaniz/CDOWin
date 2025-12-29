using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CreateClientViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service;

    // =========================
    // Input Validation
    // =========================
    //public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    // =========================
    // Constructor
    // =========================
    public CreateClientViewModel(IClientService service) {
        _service = service;
    }

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
}
