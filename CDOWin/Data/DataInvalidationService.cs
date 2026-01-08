using System;

namespace CDOWin.Data;

public class DataInvalidationService {
    public event Action? RemindersInvalidated;
    public event Action? ClientsInvalidated;

    public void InvalidateReminders() => RemindersInvalidated?.Invoke();
    public void InvalidateClients() => ClientsInvalidated?.Invoke();
}
