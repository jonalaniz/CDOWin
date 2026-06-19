using System;

namespace Backstage.Services;

public class ClientSelectionService {
    public event Action<int>? ClientSelectionRequested;

    public void RequestSelectedClient(int clientId) => ClientSelectionRequested?.Invoke(clientId);
}
