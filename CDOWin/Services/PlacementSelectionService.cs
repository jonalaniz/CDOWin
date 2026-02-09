using System;

namespace CDOWin.Services;

public class PlacementSelectionService {
    public event Action<int>? PlacementSelectionRequested;

    public void RequestSelectedPlacement(int placementID) => PlacementSelectionRequested?.Invoke(placementID);
}
