using Backstage.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.Services.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class UserViewModel : ObservableObject {
    // =========================
    // Dependencies
    // =========================
    private readonly UserService _service;
    private readonly DataCoordinator _dataCoordinator;
    private readonly DispatcherQueue _dispatcher;

    // =========================
    // Private Backing Fields
    // =========================
    private IReadOnlyList<UserSummary> _cache = [];

    // =========================
    // UI State
    // =========================
    [ObservableProperty]
    public partial ObservableCollection<UserSummary> Filtered { get; private set; } = [];

    [ObservableProperty]
    public partial UserSummary? Selected { get; set; }

    // =========================
    // Constructor
    // =========================
    public UserViewModel(DataCoordinator dataCoordinator, UserService userService) {
        _service = userService;
        _dataCoordinator = dataCoordinator;
        _dispatcher = DispatcherQueue.GetForCurrentThread();
    }

    // =========================
    // CRUD Methods
    // =========================
    public async Task LoadUserSummariesAsync(bool force = false) {
        var users = await _dataCoordinator.GetUsersAsync(force);
        if (users == null) return;

        var snapshot = users.OrderBy(u => u.FirstName).ToList().AsReadOnly();
        _cache = snapshot;

    }

    // =========================
    // Utility / Helpers
    // =========================
    private void Applyfilter() {
        string? previousSelection = Selected?.Id;

        OnUI(() => {
            Filtered = new ObservableCollection<UserSummary>(_cache);
            ReSelect(previousSelection);
        });
    }

    private void OnUI(Action action) {
        if (_dispatcher.HasThreadAccess) action();
        else _dispatcher.TryEnqueue(() => action());
    }

    private void ReSelect(string? id) {
        if (id == null) return;
        if (Filtered.FirstOrDefault(u => u.Id == id) is UserSummary selected)
            Selected = selected;
    }
}
