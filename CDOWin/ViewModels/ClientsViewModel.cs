using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
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
    public partial ObservableCollection<Invoice> Invoices { get; private set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Placement> Placements { get; private set; } = [];

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;


    // =========================
    // Constructor
    // =========================
    public ClientsViewModel(IClientService service, DataCoordinator dataCoordinator, ClientSelectionService clientSelectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

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
        if (Selected != null && Selected.Id == clientId) return;

        SearchQuery = string.Empty;
        ApplyFilter();
        _ = LoadSelectedClientAsync(clientId);
    }

    partial void OnSelectedChanged(Client? value) {
        if (value == null) return;

        // Notify the selection service
        _selectionService.SelectedClient = value;

        // Setup Placements/SAs
        if (value.Invoices is not null)
            SetupSAs(value.Invoices);

        if (value.Placements is not null)
            SetupPlacements(value.Placements);
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
    public async Task LoadClientSummariesAsync(bool force = false) {
        var clients = await _dataCoordinator.GetClientsAsync(force);
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

    public async Task<Result<Client>> UpdateClientAsync(UpdateClientDTO update) {
        if (Selected == null) return Result<Client>.Fail(new AppError(ErrorKind.Validation, "Client not selected.", null));

        var result = await _service.UpdateClientAsync(Selected.Id, update);
        if (!result.IsSuccess) return result;

        // Selected = result.Value!;
        await ReloadClientAsync();
        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    private void SetupSAs(Invoice[] invoices) {
        var sortedInvoices = invoices
            .OrderBy(i => i.EndDate)
            .Reverse()
            .ToList();

        _dispatcher.TryEnqueue(() => {
            Invoices = new ObservableCollection<Invoice>(sortedInvoices);
        });
    }

    private void SetupPlacements(Placement[] placements) {
        var sortedPlacements = placements
            .OrderBy(p => p.HireDate)
            .Reverse()
            .ToList();
        _dispatcher.TryEnqueue(() => {
            Placements = new ObservableCollection<Placement>(sortedPlacements);
        });
    }

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
        (c.Phone ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.Phone2 ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.Phone3 ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.EmploymentGoal ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (c.CaseID ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
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
