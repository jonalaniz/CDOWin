using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Export.Composer;
using CDO.Core.Export.Templates;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Data;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationsViewModel : ObservableObject {

    // =========================
    // Services / Dependencies
    // =========================
    private readonly IServiceAuthorizationService _service;
    private readonly ITemplateProvider _templateProvider = new TemplateProvider();
    private readonly DataCoordinator _dataCoordinator;
    private readonly SASelectionService _selectionService;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<ServiceAuthorization> _allServiceAuthorizations = [];

    // =========================
    // View State
    // =========================

    [ObservableProperty]
    public partial ObservableCollection<ServiceAuthorization> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial ServiceAuthorization? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================

    public ServiceAuthorizationsViewModel(DataCoordinator dataCoordinator, IServiceAuthorizationService service, SASelectionService selectionService) {
        _service = service;
        _dataCoordinator = dataCoordinator;

        _selectionService = selectionService;
        _selectionService.NewSACreated += OnNewSACreated;
        _selectionService.SASelected += OnSASelected;
    }

    // =========================
    // Selection Handlers
    // =========================

    private void OnNewSACreated() {
        _ = LoadServiceAuthorizationsAsync();
    }

    private void OnSASelected(string id) {
        Debug.WriteLine("SA SELECTED");
        var selected = _allServiceAuthorizations.FirstOrDefault(s => s.Id == id);
        if (selected == null) return;
        Debug.WriteLine("It wasnt null");
        _dispatcher.TryEnqueue(() => {
            Selected = selected;
            SearchQuery = ""; // Clearing the query applies the filter
        });
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

        // Grab the template BEFORE the thread
        var templatePath = _templateProvider.GetTemplate("Invoice.dotx"); // sync path

        var thread = new System.Threading.Thread(() => {
            try {
                Debug.WriteLine($"Opening template: {templatePath}");
                var composer = new ServiceAuthorizationComposer(Selected);
                composer.Compose(templatePath);

                tcs.SetResult(Result<string>.Success("success"));
            } catch (Exception ex) {
                tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Unknown, "Failed to export Service Authorization", Exception: ex)));
            }
        });

        thread.SetApartmentState(System.Threading.ApartmentState.STA); // MUST do before Start
        thread.Start();

        return tcs.Task;
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadServiceAuthorizationsAsync(bool force = false) {
        var serviceAuthorizations = await _dataCoordinator.GetSAsAsync();
        if (serviceAuthorizations == null) return;

        var snapshot = serviceAuthorizations.OrderBy(o => o.Id).ToList().AsReadOnly();
        _allServiceAuthorizations = snapshot;

        _dispatcher.TryEnqueue(() => {
            ApplyFilter();
        });
    }

    public async Task ReloadServiceAuthorizationAsync(string id) {
        var serviceAuthorization = await _service.GetServiceAuthorizationAsync(id);
        if (serviceAuthorization == null) return;

        var updated = _allServiceAuthorizations
            .Select(s => s.Id == id ? serviceAuthorization : s)
            .ToList()
            .AsReadOnly();

        _allServiceAuthorizations = updated;

        _dispatcher.TryEnqueue(() => {
            var index = Filtered
            .Select((s, i) => new { s, i })
            .FirstOrDefault(x => x.s.Id == id)?.i;

            if (index != null)
                Filtered[index.Value] = serviceAuthorization;

            Selected = serviceAuthorization;
        });
    }

    public async Task<Result<ServiceAuthorization>> UpdateSAAsync(UpdateServiceAuthorizationDTO update) {
        if (Selected == null) return Result<ServiceAuthorization>.Fail(new AppError(ErrorKind.Validation, "Client not selected.", null));

        var result = await _service.UpdateServiceAuthorizationAsync(Selected.Id, update);
        if (!result.IsSuccess) return result;

        await ReloadServiceAuthorizationAsync(Selected.Id);
        return result;
    }

    // =========================
    // Utility / Filtering
    // =========================
    void ApplyFilter() {
        string? previousSelection = Selected?.Id;

        if (string.IsNullOrWhiteSpace(SearchQuery)) {
            Filtered = new ObservableCollection<ServiceAuthorization>(_allServiceAuthorizations);
            ReSelect(previousSelection);
            return;
        }

        var query = SearchQuery.Trim().ToLower();
        var result = _allServiceAuthorizations.Where(s =>
        (s.Client?.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.Client?.CounselorReference?.Name ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.Id ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        (s.Description ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
        );

        Filtered = new ObservableCollection<ServiceAuthorization>(result);
        ReSelect(previousSelection);

    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(sa => sa.Id == id) is ServiceAuthorization selected)
            Selected = selected;
    }
}
