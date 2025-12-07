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
    public partial ObservableCollection<ClientSummaryDTO> AllClientSummaries { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> FilteredClients { get; private set; } = [];

    [ObservableProperty]
    public partial Client? SelectedClient { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;


    // Constructor
    public ClientsViewModel(IClientService service) {
        _service = service;
    }

    // Change tracking methods
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            Debug.WriteLine(SelectedClient.reminders);
    }

    // Utility Methods
    void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            FilteredClients = new ObservableCollection<ClientSummaryDTO>(AllClientSummaries);
            return;
        }

        var query = SearchQuery.Trim().ToLower();

        var result = AllClientSummaries.Where(c =>
        (c.name?.ToLower().Contains(query) ?? false)
        || (c.id.ToString()?.Contains(query) ?? false)
        || (c.formattedAddress?.ToLower().Contains(query) ?? false)
        || (c.counselorName?.ToLower().Contains(query) ?? false)
        );

        FilteredClients = new ObservableCollection<ClientSummaryDTO>(result);
    }

    // CRUD Methods
    public async Task LoadClientSummariesAsync() {
        var clients = await _service.GetAllClientSummariesAsync();

        // Sort that list of downloaded Clients
        List<ClientSummaryDTO> SortedClients = clients.OrderBy(o => o.name).ToList();

        // Clear the ViewModel's Clients variable
        AllClientSummaries.Clear();

        // Loop over and add all of the clients in the sorted clients list to the main Clients variable
        foreach (var client in SortedClients) {
            AllClientSummaries.Add(client);
        }

        FilteredClients = new ObservableCollection<ClientSummaryDTO>(AllClientSummaries);
    }

    public async Task ClientSelected(int id) {
        // Ensure the Selected Client is not being called twice.
        if (SelectedClient != null && SelectedClient.id == id)
            return;

        // Fetch the full client.
        var selectedClient = await _service.GetClientAsync(id);
        SelectedClient = selectedClient;
    }

    public async Task UpdateClient(UpdateClientDTO update) {
        if (SelectedClient == null)
            return;
        var updatedClient = await _service.UpdateClientAsync(SelectedClient.id, update);
        SelectedClient = updatedClient;
    }
}
