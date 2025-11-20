using CDO.Core.Models;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ClientsViewModel : ObservableObject {
    private readonly IClientService _service;

    [ObservableProperty]
    public partial ObservableCollection<Client> Clients { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Client> SortedClients { get; set; } = [];

    [ObservableProperty]
    public partial Client? SelectedClient { get; set; }

    public ClientsViewModel(IClientService service) {
        _service = service;
    }

    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            _ = RefreshSelectedClient(value.id);
    }

    public async Task LoadClientsAsync() {
        var clients = await _service.GetAllClientsAsync();
        List<Client> SortedClients = clients.OrderBy(o => o.name).ToList();
        Clients.Clear();

        foreach (var client in SortedClients) {
            Clients.Add(client);
        }
    }

    public async Task RefreshSelectedClient(int id) {
        var client = await _service.GetClientAsync(id);
        if (SelectedClient != client) {
            SelectedClient = client;

            var index = Clients.IndexOf(Clients.First(c => c.id == id));
            Clients[index] = client;
        }
    }
}
