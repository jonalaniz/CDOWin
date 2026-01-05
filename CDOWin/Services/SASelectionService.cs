using System;

namespace CDOWin.Services;

public class SASelectionService {
    public event Action<string>? SASelected;
    public event Action? NewSACreated;

    public void RequestSelectedSA(string id) {
        SASelected?.Invoke(id);
    }

    public void NotifyNewSACreated() {
        NewSACreated?.Invoke();
    }
}
