using CDO.Core.DTOs.Clients;
using System;

namespace CDOWin.Services;

public class ClientSelectionService {
    public event Action<ClientDetail?>? SelectedClientChanged;
    public event Action<int>? ClientSelectionRequested;
    public event Action? NewReminderCreated;
    private ClientDetail? _selectedClient;

    public ClientDetail? SelectedClient {
        get => _selectedClient;
        set {
            if (_selectedClient != value) {
                _selectedClient = value;
                SelectedClientChanged?.Invoke(value);
            }
        }
    }

    public void RequestSelectedClient(int clientId) => ClientSelectionRequested?.Invoke(clientId);

    public void NotifyNewReminderCreated() => NewReminderCreated?.Invoke();
}
