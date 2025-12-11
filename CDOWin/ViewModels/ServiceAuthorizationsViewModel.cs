using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationsViewModel : ObservableObject {
    private readonly IServiceAuthorizationService _service;

    [ObservableProperty]
    public partial ObservableCollection<ServiceAuthorization> ServiceAuthorizations { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ServiceAuthorization> FilteredServiceAuthorizations { get; private set; } = [];

    [ObservableProperty]
    public partial ServiceAuthorization? SelectedServiceAuthorization { get; set; }

    public ServiceAuthorizationsViewModel(IServiceAuthorizationService service) {
        _service = service;
    }

    partial void OnSelectedServiceAuthorizationChanged(ServiceAuthorization? value) {
        if (value != null)
            _ = RefreshSelectedServiceAuthorization(value.id);
    }

    public async Task LoadServiceAuthorizationsAsync() {
        var serviceAuthorizations = await _service.GetAllServiceAuthorizationsAsync();
        List<ServiceAuthorization> SortedServiceAuthorizations = serviceAuthorizations.OrderBy(o => o.clientID).ToList();
        ServiceAuthorizations.Clear();

        foreach (var serviceAuthorization in SortedServiceAuthorizations) {
            ServiceAuthorizations.Add(serviceAuthorization);
        }
    }

    public async Task RefreshSelectedServiceAuthorization(string id) {
        var po = await _service.GetServiceAuthorizationAsync(id);
        if (SelectedServiceAuthorization != po) {
            SelectedServiceAuthorization = po;

            var index = ServiceAuthorizations.IndexOf(ServiceAuthorizations.First(p => p.id == id));
            ServiceAuthorizations[index] = po;
        }
    }
}
