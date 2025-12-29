using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CreateServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;

    // =========================
    // Input Validation
    // =========================
    //public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    // =========================
    // Constructor
    // =========================
    public CreateServiceAuthorizationsViewModel(IServiceAuthorizationService service) {
        _service = service;
    }

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
}
