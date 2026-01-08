using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class StatesViewModel(DataCoordinator dataCoordinator, IStateService service) : ObservableObject {
    private readonly IStateService _service = service;
    private readonly DataCoordinator _dataCoordinator = dataCoordinator;

    [ObservableProperty]
    public partial ObservableCollection<State> States { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<State> FilteredStates { get; private set; } = [];

    [ObservableProperty]
    public partial State? SelectedState { get; set; }

    partial void OnSelectedStateChanged(State? value) {
        if (value == null) return;
        _ = RefreshSelectedState(value.Id);
    }

    public async Task LoadStatesAsync() {
        var states = await _dataCoordinator.GetStatesAsync();
        if (states == null) return;
        List<State> SortedStates = states.OrderBy(o => o.Name).ToList();
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

            var index = States.IndexOf(States.First(s => s.Id == id));
            States[index] = state;
        }
    }
}
