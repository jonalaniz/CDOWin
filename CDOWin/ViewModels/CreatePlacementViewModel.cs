using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CreatePlacementViewModel(IPlacementService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IPlacementService _service = service;

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
}
