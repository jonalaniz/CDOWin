using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class StatesViewModel : ObservableObject {
    private readonly IStateService _service;

    [ObservableProperty]
    public partial ObservableCollection<State> States { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<State> FilteredStates { get; private set; } = [];

    [ObservableProperty]
    public partial State? SelectedState { get; set; }

    public StatesViewModel(IStateService service) {
        _service = service;
    }

    partial void OnSelectedStateChanged(State? value) {
        if (value == null) return;
        _ = RefreshSelectedState(value.id);
    }

    public async Task LoadStatesAsync() {
        var states = await _service.GetAllStatesAsync();
        if (states == null) return;
        List<State> SortedStates = states.OrderBy(o => o.name).ToList();
        States.Clear();

        foreach (var state in SortedStates) {
            States.Add(state);
        }
    }

    public async Task RefreshSelectedState(int id) {
        var state = await _service.GetStateAsync(id);
        if (state == null) return;
        if (SelectedState != state) {
            SelectedState = state;

            var index = States.IndexOf(States.First(s => s.id == id));
            States[index] = state;
        }
    }
}
