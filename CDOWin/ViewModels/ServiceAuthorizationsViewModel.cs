using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<ServiceAuthorization> _allServiceAuthorizations = [];

    // =========================
    // View State
    // =========================
   
    [ObservableProperty]
    public partial ObservableCollection<ServiceAuthorization> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ServiceAuthorization? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public ServiceAuthorizationsViewModel(IServiceAuthorizationService service) {
        _service = service;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        if (_dispatcher.HasThreadAccess)
            ApplyFilter();
        else
            _dispatcher.TryEnqueue(ApplyFilter);
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadServiceAuthorizationsAsync() {
        var serviceAuthorizations = await _service.GetAllServiceAuthorizationsAsync();
        if (serviceAuthorizations == null) return;

        var snapshot = serviceAuthorizations.OrderBy(o => o.id).ToList().AsReadOnly();
        _allServiceAuthorizations = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadServiceAuthorizationAsync(string id) {
        var serviceAuthorization = await _service.GetServiceAuthorizationAsync(id);
        if (serviceAuthorization == null) return;

        var updated = _allServiceAuthorizations
            .Select(s => s.id == id ? serviceAuthorization : s)
            .ToList()
            .AsReadOnly();

        _allServiceAuthorizations = updated;
        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
            Selected = serviceAuthorization;
        });

        Selected = serviceAuthorization;
    }

    // =========================
    // Utility / Filtering
    // =========================
    void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<ServiceAuthorization>(_allServiceAuthorizations);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allServiceAuthorizations.Where(s =>
        (s.client?.name?.ToLower().Contains(query) ?? false)
        || (s.client?.counselorReference?.name?.ToLower().Contains(query) ?? false)
        || (s.id?.ToLower().Contains(query) ?? false)
        || (s.description?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<ServiceAuthorization>(result);
    }
}
