using System;

namespace CDOWin.Services;

public class PlacementSelectionService {
    public event Action<string>? PlacementSelected;
    public event Action? NewPlacementCreated;

    public void RequestSelectedPlacement(string id) {
        PlacementSelected?.Invoke(id);
    }

    public void NotifyNewSACreated() {
        NewPlacementCreated?.Invoke();
    }
}
