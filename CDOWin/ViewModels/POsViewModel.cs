using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class POsViewModel : ObservableObject {
    private readonly IPOService _service;

    [ObservableProperty]
    public partial ObservableCollection<PO> POs { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<PO> FilteredPOs { get; private set; } = [];

    [ObservableProperty]
    public partial PO? SelectedPO { get; set; }

    public POsViewModel(IPOService service) {
        _service = service;
    }

    partial void OnSelectedPOChanged(PO? value) {
        if (value != null)
            _ = RefreshSelectedPO(value.id);
    }

    public async Task LoadPOsAsync() {
        var pos = await _service.LoadPOsAsync();
        List<PO> SortedPOs = pos.OrderBy(o => o.clientID).ToList();
        POs.Clear();

        foreach (var po in SortedPOs) {
            POs.Add(po);
        }
    }

    public async Task RefreshSelectedPO(string id) {
        var po = await _service.LoadPOAsync(id);
        if (SelectedPO != po) {
            SelectedPO = po;

            var index = POs.IndexOf(POs.First(p => p.id == id));
            POs[index] = po;
        }
    }
}
