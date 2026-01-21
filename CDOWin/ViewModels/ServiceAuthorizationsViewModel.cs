using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Composers;
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

public partial class ServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly ClientSelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<Invoice> _allServiceAuthorizations = [];

    // =========================
    // View State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<Invoice> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial Invoice? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================

    public ServiceAuthorizationsViewModel(DataCoordinator dataCoordinator, IServiceAuthorizationService service, ClientSelectionService selectionService) {
        _service = service;
        _selectionService = selectionService;
        _dataCoordinator = dataCoordinator;
    }

    // =========================
    // Public Methods
    // =========================
    public void RequestClient(int clientID) {
        AppServices.Navigation.Navigate(Views.CDOFrame.Clients);
        _selectionService.RequestSelectedClient(clientID);
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

    // =========================
    // Export Methods
    // =========================
    public Task<Result<string>> ExportSelectedAsync() {
        var tcs = new TaskCompletionSource<Result<string>>();

        if (Selected == null) {
            tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Validation, "No SA Selected.")));
            return tcs.Task;
        }

        if (Selected.Client is not Client client) {
            tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Validation, "This shouldn't be possible")));
            return tcs.Task;
        }

        var composer = new ServiceAuthorizationComposer(Selected);
        return composer.Compose();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadServiceAuthorizationsAsync(bool force = false) {
        var serviceAuthorizations = await _dataCoordinator.GetSAsAsync();
        if (serviceAuthorizations == null) return;

        var snapshot = serviceAuthorizations.OrderBy(o => o.ServiceAuthorizationNumber).ToList().AsReadOnly();
        _allServiceAuthorizations = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadServiceAuthorizationAsync(string id) {
        var serviceAuthorization = await _service.GetServiceAuthorizationAsync(id);
        if (serviceAuthorization == null) return;

        var updated = _allServiceAuthorizations
            .Select(s => s.ServiceAuthorizationNumber == id ? serviceAuthorization : s)
            .ToList()
            .AsReadOnly();

        _allServiceAuthorizations = updated;

        _dispatcher.TryEnqueue(() => {
            var index = Filtered
            .Select((s, i) => new { s, i })
            .FirstOrDefault(x => x.s.ServiceAuthorizationNumber == id)?.i;

            if (index != null)
                Filtered[index.Value] = serviceAuthorization;

            Selected = serviceAuthorization;
        });
    }

    public async Task<Result<bool>> DeleteSelectedSA() {
        if (Selected == null) return Result<bool>.Fail(new AppError(ErrorKind.Validation, "No Placement selected.", null, null));
        var id = Selected.ServiceAuthorizationNumber;
        var result = await _service.DeleteServiceAuthorizationAsync(id);

        if (result.IsSuccess) {
            Selected = null;
            _allServiceAuthorizations = _allServiceAuthorizations
                .Where(sa => sa.ServiceAuthorizationNumber != id)
                .ToList()
                .AsReadOnly();
            ApplyFilter();
            _ = LoadServiceAuthorizationsAsync(force: true);
        }

        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    void ApplyFilter() {
        string? previousSelection = Selected?.ServiceAuthorizationNumber;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<Invoice>(_allServiceAuthorizations);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allServiceAuthorizations.Where(s =>
        (s.Client?.NameAndID ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.Client?.CounselorReference?.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.ServiceAuthorizationNumber ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.Description ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<Invoice>(result);
        ReSelect(previousSelection);

    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(sa => sa.ServiceAuthorizationNumber == id) is Invoice selected)
            Selected = selected;
    }
}
