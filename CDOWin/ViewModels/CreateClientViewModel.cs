using CDO.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CreateClientViewModel(IClientService service) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly IClientService _service = service;

    // =========================
    // Property Change Methods
    // =========================

    // =========================
    // CRUD Methods
    // =========================
}
