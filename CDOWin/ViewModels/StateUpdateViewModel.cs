using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class StateUpdateViewModel(State state) : ObservableObject {
    public State Original = state;
    public UpdateStateDTO Updated = new();
}

