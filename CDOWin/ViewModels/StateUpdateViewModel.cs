using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.ViewModels;

public partial class StateUpdateViewModel: ObservableObject {
    public State Original;
    public UpdateStateDTO Updated = new UpdateStateDTO();

    public StateUpdateViewModel(State state) {
        Original = state;
    }
}

