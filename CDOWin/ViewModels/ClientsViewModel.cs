using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
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
    private readonly ClientSelectionService _selectionService;

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
    public ClientsViewModel(IClientService service, Services.ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _selectionService.ClientSelectionRequested += OnRequestSelectedClientChange;
    }

    // Change tracking methods
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    private void OnRequestSelectedClientChange(int clientId) {
        if (SelectedClient != null && SelectedClient.id == clientId)
            return;

        SearchQuery = string.Empty;
        ApplyFilter();
        _ = ClientSelected(clientId);
    }

    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            _selectionService.SelectedClient = value;
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

        ApplyFilter();
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

    public async Task ReloadClientAsync() {
        if (SelectedClient == null)
            return;
        SelectedClient = await _service.GetClientAsync(SelectedClient.id);
    }
}
