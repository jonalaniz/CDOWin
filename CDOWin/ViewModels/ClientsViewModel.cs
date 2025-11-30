using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ClientsViewModel : ObservableObject {

    // Private service, used to make network calls.
    private readonly IClientService _service;

    // Observable Properties

    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> ClientSummaries { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Client> FilteredClients { get; private set; } = [];

    [ObservableProperty]
    public partial Client? SelectedClient { get; set; }


    // Constructor
    public ClientsViewModel(IClientService service) {
        _service = service;
    }

    // OnSelectedClientChanged - Testing: Pulling reminders
    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            Debug.WriteLine(SelectedClient.reminders);
    }

    // CRUD Methods

    public async Task LoadClientSummariesAsync() {
        var clients = await _service.GetAllClientSummariesAsync();

        // Sort that list of downloaded Clients
        List<ClientSummaryDTO> SortedClients = clients.OrderBy(o => o.name).ToList();

        // Clear the ViewModel's Clients variable
        ClientSummaries.Clear();

        // Loop over and add all of the clients in the sorted clients list to the main Clients variable
        foreach (var client in SortedClients) {
            ClientSummaries.Add(client);
        }
    }

    public async Task ClientSelected(int id) {
        // Ensure the Selected Client is not being called twice.
        if (SelectedClient != null && SelectedClient.id == id)
            return;

        // Fetch the full client.
        var selectedClient = await _service.GetClientAsync(id);
        SelectedClient = selectedClient;
    }
}
