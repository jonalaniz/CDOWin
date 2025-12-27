using CDO.Core.Models;
using System;
using System.Diagnostics;

namespace CDOWin.Services;

public class ClientSelectionService {
    public event Action<Client?>? SelectedClientChanged;
    public event Action<int>? ClientSelectionRequested;
    public event Action? NewReminderCreated;
    private Client? _selectedClient;

    public Client? SelectedClient {
        get => _selectedClient;
        set {
            if (_selectedClient != value) {
                _selectedClient = value;
                SelectedClientChanged?.Invoke(value);
            }
        }
    }

    public void RequestSelectedClient(int clientId) {
        ClientSelectionRequested?.Invoke(clientId);
    }

    public void NotifyNewReminderCreated() {
        Debug.WriteLine("Notified");
        NewReminderCreated?.Invoke();
    }
}
