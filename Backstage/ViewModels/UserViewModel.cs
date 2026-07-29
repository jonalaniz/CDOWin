using Backstage.Data;
using Backstage.Services;
using CDO.Core.DTOs.Admin;
using CDO.Core.ErrorHandling;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class UserViewModel : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly UserService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly UserSelectionService _userSelectionService;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private CancellationTokenSource? _filterCts;

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<UserSummary> Users { get; private set; } = [];

    [ObservableProperty]
    public partial UserSummary? Selected { get; set; }

    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    // =========================
    // Constructor
    // =========================
    public UserViewModel(DataCoordinator dataCoordinator, UserSelectionService userSelectionService, UserService userService) {
        _service = userService;
        _userSelectionService = userSelectionService;
        _dataCoordinator = dataCoordinator;
        _dispatcher = DispatcherQueue.GetForCurrentThread();

        _userSelectionService.UserSelectionRequested += OnRequestSelectedUser;
    }

    // =========================
    // Property Change Methods
    // =========================
    partial void OnSearchQueryChanged(string value) => _ = RefreshAsync();

    private void OnRequestSelectedUser(string userId) {
        if (Selected != null && Selected.Id == userId) return;
        SearchQuery = string.Empty;
        OnUI(() => {
            if (Users.FirstOrDefault(u => u.Id == userId) is UserSummary summary)
                Selected = summary;
        }
        );
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task RefreshAsync(bool force = false) {
        _filterCts?.Cancel();
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        try {
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            var snapshot = await _dataCoordinator.GetUsersAsync(force);
            if (token.IsCancellationRequested) return;

            string? previousSelection = Selected?.Id;

            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                var query = SearchQuery.Trim().ToLower();
                snapshot = snapshot.Where(u =>
                u.Username.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (u.FirstName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                (u.LastName ?? "").Contains(query, StringComparison.CurrentCultureIgnoreCase)
                ).ToList();
            }

            OnUI(() => {
                Users = new ObservableCollection<UserSummary>(snapshot);
                ReSelect(previousSelection);
            });
        } catch (OperationCanceledException) { }
    }

    // =========================
    // Utility / Helpers
    // =========================

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Users.FirstOrDefault(u => u.Id == id) is UserSummary selected)
            Selected = selected;
    }
}
