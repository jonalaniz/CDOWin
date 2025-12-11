using CDO.Core.Models;
using System;

namespace CDOWin.Services {
    public class ClientSelectionService {
        public event Action<Client?> SelectedClientChanged;
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
    }
}
