using CDO.Core.Models;
using CDO.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public class ClientsViewModel : INotifyPropertyChanged {
    private readonly IClientService _service;

    public ObservableCollection<Client> Clients { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public ClientsViewModel(IClientService service) {
        _service = service;
    }

    public async Task LoadClientsAsync() {
        var clients = await _service.GetAllClientsAsync();
        List<Client> SortedClients = clients.OrderBy(o => o.name).ToList();
        Clients.Clear();

        foreach (var client in SortedClients) {
            Clients.Add(client);
        }
    }

    protected void Notify([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
