using CDO.Core.Models;
using CDOWin.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class StatesViewModel(DataCoordinator dataCoordinator) : ObservableObject {
    private readonly DataCoordinator _dataCoordinator = dataCoordinator;

    [ObservableProperty]
    public partial ObservableCollection<State> States { get; private set; } = [];

    public List<State> GetStates() {
        if (States.Count == 0)
            LoadStatesAsync().GetAwaiter().GetResult();

        return States.ToList();
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
}
