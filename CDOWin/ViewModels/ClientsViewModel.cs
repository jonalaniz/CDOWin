using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ClientsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IClientService _service;
    private readonly ClientSelectionService _selectionService;

    // =========================
    // View State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> AllClientSummaries { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> FilteredClients { get; private set; } = [];

    [ObservableProperty]
    public partial Client? SelectedClient { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;


    // =========================
    // Constructor
    // =========================
    public ClientsViewModel(IClientService service, ClientSelectionService clientSelectionService) {
        _service = service;
        _selectionService = clientSelectionService;
        _selectionService.ClientSelectionRequested += OnRequestSelectedClientChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        ApplyFilter();
    }

    private void OnRequestSelectedClientChange(int clientId) {
        if (SelectedClient != null && SelectedClient.id == clientId)
            return;

        SearchQuery = string.Empty;
        ApplyFilter();
        _ = LoadSelectedClientAsync(clientId);
    }

    partial void OnSelectedClientChanged(Client? value) {
        if (value != null)
            _selectionService.SelectedClient = value;
    }

    // =========================
    // Public Methods
    // =========================
    public void NotifyNewClientCreated() {
        _selectionService.NotifyNewReminderCreated();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadClientSummariesAsync() {
        var clients = await _service.GetAllClientSummariesAsync();
        if (clients == null) return;

        List<ClientSummaryDTO> SortedClients = clients.OrderBy(o => o.name).ToList();
        AllClientSummaries.Clear();
        foreach (var client in SortedClients) {
            AllClientSummaries.Add(client);
        }

        ApplyFilter();
    }

    public async Task LoadSelectedClientAsync(int id) {
        if (SelectedClient != null && SelectedClient.id == id) return;

        var selectedClient = await _service.GetClientAsync(id);
        SelectedClient = selectedClient;
    }

    public async Task ReloadClientAsync() {
        if (SelectedClient == null) return;
        SelectedClient = await _service.GetClientAsync(SelectedClient.id);
    }

    public async Task UpdateClientAsync(UpdateClientDTO update) {
        if (SelectedClient == null) return;
        var updatedClient = await _service.UpdateClientAsync(SelectedClient.id, update);
        SelectedClient = updatedClient;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
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
}
