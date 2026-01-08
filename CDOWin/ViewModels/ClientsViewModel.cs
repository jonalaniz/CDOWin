using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
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
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _selectionService;
    private readonly PlacementSelectionService _placementSelectionService;
    private readonly SASelectionService _saSelectionService;
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
    public partial ClientSummaryDTO? SelectedSummary { get; set; }

    [ObservableProperty]
    public partial Client? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;


    // =========================
    // Constructor
    // =========================
    public ClientsViewModel(IClientService service, DataCoordinator dataCoordinator, ClientSelectionService clientSelectionService, PlacementSelectionService psService, SASelectionService saService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _placementSelectionService = psService;
        _saSelectionService = saService;

        _selectionService = clientSelectionService;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _selectionService.ClientSelectionRequested += OnRequestSelectedClientChange;
    }



    // =========================
    // Child Model Selection
    // =========================

    public void PlacementSelected(string id) {
        _placementSelectionService.RequestSelectedPlacement(id);
    }

    public void SASelected(string id) {
        _saSelectionService.RequestSelectedSA(id);
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
        if (Selected != null && Selected.Id == clientId)
            return;

        SearchQuery = string.Empty;
        ApplyFilter();
        _ = LoadSelectedClientAsync(clientId);
    }

    partial void OnSelectedChanged(Client? value) {
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
        var clients = await _dataCoordinator.GetClientsAsync();
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.Name).ToList().AsReadOnly();
        _allClients = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task LoadSelectedClientAsync(int id) {
        if (Selected != null && Selected.Id == id) return;

        var selectedClient = await _service.GetClientAsync(id);
        Selected = selectedClient;
    }

    public async Task ReloadClientAsync() {
        if (Selected == null) return;
        Selected = await _service.GetClientAsync(Selected.Id);
    }

    public async Task UpdateClientAsync(UpdateClientDTO update) {
        if (Selected == null) return;
        var updatedClient = await _service.UpdateClientAsync(Selected.Id, update);
        Selected = updatedClient;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void ApplyFilter() {
        int? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<ClientSummaryDTO>(_allClients);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();

        var result = _allClients.Where(c =>
        (c.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.Id.ToString() ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.FormattedAddress ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.CounselorName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<ClientSummaryDTO>(result);
        ReSelect(previousSelection);
    }

    private void ReSelect(int? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(c => c.Id == id) is ClientSummaryDTO selected)
            SelectedSummary = selected;
    }
}
