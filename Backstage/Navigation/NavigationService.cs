using Backstage.Views;
using CDO.Abstractions.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Backstage.Navigation;

public partial class NavigationService : ObservableObject, INavigationService<BackstageView> {

    // =========================
    // Dependencies
    // =========================
    private NavigationView? _navigationView;
    private Frame? _frame;

    // =========================
    // State
    // =========================
    private BackstageView? _currentView;
    private readonly Dictionary<BackstageView, Type> _pages = new() {
        [BackstageView.Home] = typeof(HomePage),
        [BackstageView.Billing] = typeof(BillingPage),
        [BackstageView.Clients] = typeof(ClientsPage),
        [BackstageView.Users] = typeof(UsersPage),
        [BackstageView.Settings] = typeof(SettingsPage),
    };
    private readonly ObservableCollection<BackstageView> _history = [];

    // =========================
    // Public Fields
    // =========================
    public bool CanGoBack => _history.Count != 0;

    // =========================
    // Constructor
    // =========================
    public NavigationService() {
        _history.CollectionChanged += (_, _) => OnPropertyChanged(nameof(CanGoBack));
    }

    // =========================
    // Property Change Methods
    // =========================
    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (_frame == null ||
            args.SelectedItem is not NavigationViewItem { Tag: BackstageView view } ||
            view == _currentView) return;

        NavigateTo(view);
        if (_currentView is BackstageView oldFrame) { _history.Add(oldFrame); }
        _currentView = view;
    }


    // =========================
    // Public Methods
    // =========================
    public void Initialize(NavigationView navigationView, Frame frame) {
        _navigationView = navigationView;
        _frame = frame;
        _navigationView.SelectionChanged += OnSelectionChanged;
    }

    public void BackRequested() {
        if (_history.Count == 0 || _frame == null) return;

        // Remove the last frame and navigate to it
        var view = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        NavigateTo(view);

        // Set our current view
        _currentView = view;

        // Grabs the item from NavigationView and selects it.
        SelectView(view);
    }

    public void RequestNavigation(BackstageView view) {
        if (_frame == null || _navigationView == null) return;
        NavigateTo(view);
        SelectView(view);
    }

    // =========================
    // Utility Methods
    // =========================
    private void NavigateTo(BackstageView view) {
        if (_frame == null || !_pages.TryGetValue(view, out var pageType)) return;
        _frame.Navigate(pageType);
    }

    private void SelectView(BackstageView view) {
        if (_navigationView == null) return;
        var item = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is BackstageView viewItem && view == viewItem);
        if (item != null) _navigationView.SelectedItem = item;
    }
}
