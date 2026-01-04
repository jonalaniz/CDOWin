using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
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
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<ClientSummaryDTO> _allClients = [];

    // =========================
    // Public Property / State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<ClientSummaryDTO> Filtered { get; private set; } = [];

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
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.ClientSelectionRequested += OnRequestSelectedClientChange;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) {
        if (_dispatcher.HasThreadAccess)
            ApplyFilter();
        else
            _dispatcher.TryEnqueue(ApplyFilter);
    }

    private void OnRequestSelectedClientChange(int clientId) {
        if (SelectedClient != null && SelectedClient.Id == clientId)
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

        var snapshot = clients.OrderBy(c => c.Name).ToList().AsReadOnly();
        _allClients = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task LoadSelectedClientAsync(int id) {
        if (SelectedClient != null && SelectedClient.Id == id) return;

        var selectedClient = await _service.GetClientAsync(id);
        SelectedClient = selectedClient;
    }

    public async Task ReloadClientAsync() {
        if (SelectedClient == null) return;
        SelectedClient = await _service.GetClientAsync(SelectedClient.Id);
    }

    public async Task UpdateClientAsync(UpdateClientDTO update) {
        if (SelectedClient == null) return;
        var updatedClient = await _service.UpdateClientAsync(SelectedClient.Id, update);
        SelectedClient = updatedClient;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<ClientSummaryDTO>(_allClients);
            return;
        }

        var query = SearchQuery.Trim().ToLower();

        var result = _allClients.Where(c =>
        (c.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)  || 
        (c.Id.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)  || 
        (c.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)  || 
        (c.CounselorName ??"").Contains(query, StringComparison.CurrentCultureIgnoreCase) 
        );

        Filtered = new ObservableCollection<ClientSummaryDTO>(result);
    }
}
