using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
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

    // =========================
    // View State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<ServiceAuthorization> All { get; private set; } = [];

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
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadServiceAuthorizationsAsync() {
        var serviceAuthorizations = await _service.GetAllServiceAuthorizationsAsync();
        if (serviceAuthorizations == null) return;

        List<ServiceAuthorization> SortedServiceAuthorizations = serviceAuthorizations.OrderBy(o => o.clientID).ToList();
        All.Clear();

        foreach (var serviceAuthorization in SortedServiceAuthorizations) {
            All.Add(serviceAuthorization);
        }

        ApplyFilter();
    }

    public async Task ReloadServiceAuthorizationAsync(string id) {
        var serviceAuthorization = await _service.GetServiceAuthorizationAsync(id);
        if (serviceAuthorization == null) return;
        var index = All.IndexOf(All.First(p => p.id == id));
        All[index] = serviceAuthorization;
        Selected = serviceAuthorization;
    }

    // =========================
    // Utility / Filtering
    // =========================
    void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<ServiceAuthorization>(All);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = All.Where(s =>
        (s.client?.name?.ToLower().Contains(query) ?? false)
        || (s.client?.counselorReference?.name?.ToLower().Contains(query) ?? false)
        || (s.id?.ToLower().Contains(query) ?? false)
        || (s.description?.ToLower().Contains(query) ?? false)
        );

        Filtered = new ObservableCollection<ServiceAuthorization>(result);
    }
}
