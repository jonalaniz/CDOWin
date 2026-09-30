using Backstage.Composers;
using Backstage.Data;
using Backstage.Services;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class HomeViewModel(DataCoordinator dataCoordinator, ClientSelectionService selectionService) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly DataCoordinator _dataCoordinator = dataCoordinator;
    private readonly ClientSelectionService _selectionService = selectionService;
    private readonly DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial DailySnapshot? Snapshot { get; private set; } = null;

    [ObservableProperty]
    public partial ObservableCollection<AdminClientSummary> StaleClients { get; private set; } = [];

    [ObservableProperty]
    public partial DateOnly Date { get; private set; } = DateOnly.FromDateTime(DateTime.Today);

    // =========================
    // Public Methods
    // =========================
    public void RequestClient(int clientId) => _selectionService.RequestSelectedClient(clientId);

    public async Task AddDaysAsync(int days) {
        var newDate = Date.AddDays(days);
        if (newDate > DateOnly.FromDateTime(DateTime.Today)) return;
        Date = newDate;
        await LoadSnapshotAsync();
    }

    public async Task<Result> ExportSAs() {
        var list = await _dataCoordinator.GetExpiringSAsAsync();
        if (list == null) return Result.Fail(new AppError(ErrorKind.Unknown, "SA Export empty", null));
        var composer = new SAComposer();
        composer.BuildCSV(list.ToList());
        return Result.Success();
    }

    // =========================
    // Get Methods
    // =========================
    public async Task LoadSnapshotAsync() {
        var date = Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        var snapshot = await _dataCoordinator.GetSnapshotAsync(date);
        if (snapshot == null) return;

        OnUI(() => {
            Snapshot = snapshot;
        });
    }

    public async Task LoadStaleClientsAsync(bool force = false) {
        var clients = await _dataCoordinator.GetStaleClientsAsync(force);
        if (clients == null) return;

        var snapshot = clients.OrderBy(c => c.UpdatedAt).ToList().AsReadOnly();
        OnUI(() => {
            StaleClients = new ObservableCollection<AdminClientSummary>(snapshot);
        });
    }

    // =========================
    // Utility Methods
    // =========================
    public void RemoveClient(int id) {
        if (StaleClients.FirstOrDefault(c => c.Id == id) is not AdminClientSummary client) return;
        OnUI(() => StaleClients.Remove(client));
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }
}
