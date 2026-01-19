using System;

namespace CDOWin.Data;

public class DataInvalidationService {
    public event Action? SAsInvalidated;
    public event Action? PlacementsInvalidated;

    public void InvalidateSAs() => SAsInvalidated?.Invoke();
    public void InvalidatePlacements() => PlacementsInvalidated?.Invoke();
}
