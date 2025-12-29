using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CreatePlacementViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IPlacementService _service;

    // =========================
    // Input Validation
    // =========================
    //public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    // =========================
    // Constructor
    // =========================
    public CreatePlacementViewModel(IPlacementService service) {
        _service = service;
    }

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
}
