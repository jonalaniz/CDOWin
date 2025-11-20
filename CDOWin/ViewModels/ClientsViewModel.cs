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

    // Private service, used to make network calls.
    private readonly IClientService _service;

    // Observable Properties

    [ObservableProperty]
    public partial ObservableCollection<Client> Clients { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Client> FilteredClients { get; private set; } = [];

    [ObservableProperty]
    public partial Client? SelectedClient { get; set; }


    // Constructor
    public ClientsViewModel(IClientService service) {
        _service = service;
    }

    // OnSelectedClientChanged - We are overriding the generated one to refresh our selected client
    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            _ = RefreshSelectedClient(value.id);
    }

    // CRUD Methods

    public async Task LoadClientsAsync() {
        // Get all of the Clients and assign them to a variable
        var clients = await _service.GetAllClientsAsync();

        // Sort that list of downloaded Clients
        List<Client> SortedClients = clients.OrderBy(o => o.name).ToList();

        // Clear the ViewModel's Clients variable
        Clients.Clear();

        // Loop over and add all of the clients in the sorted clients list to the main Clients variable
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
