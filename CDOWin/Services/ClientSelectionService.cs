using CDO.Core.Models;
using System;

namespace CDOWin.Services;

public class ClientSelectionService {
    public event Action<Client?>? SelectedClientChanged;
    public event Action<int>? ClientSelectionRequested;
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
}
