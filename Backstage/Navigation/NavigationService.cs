using Backstage.Views;
using CDO.Abstractions.Navigation;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backstage.Navigation;

public sealed class NavigationService : INavigationService<BackstageView> {
    private NavigationView? _navigationView;
    private Frame? _frame;
    private readonly Dictionary<BackstageView, Type> _pages = new();
    private BackstageView? _currentFrame;
    private List<BackstageView> _history = [];

    public void Initialize(NavigationView navigationView, Frame frame) {
        _navigationView = navigationView;
        _frame = frame;

        // Seed our page types
        _pages[BackstageView.Home] = typeof(HomePage);
        _pages[BackstageView.Billing] = typeof(BillingPage);
        _pages[BackstageView.Clients] = typeof(ClientsPage);
        _pages[BackstageView.Users] = typeof(UsersPage);
        _pages[BackstageView.Settings] = typeof(SettingsPage);

        _navigationView.SelectionChanged += SelectionChanged;
    }

    private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (_frame == null) return;
        if (args.SelectedItem is not NavigationViewItem { Tag: BackstageView frame }) return;
        if (frame == _currentFrame) return;
        if (!_pages.TryGetValue(frame, out var page)) return;

        _frame.Navigate(page);

        if (_currentFrame is BackstageView oldFrame)
            _history.Add(oldFrame);

        _currentFrame = frame;
    }

    public void BackRequested() {
        if (_history.Count == 0) return;
        if (_frame == null) return;
        var frame = _history.LastOrDefault();
        _history.RemoveAt(_history.Count - 1);

        if (_navigationView == null) return;
        if (!_pages.TryGetValue(frame, out var page)) return;

        _frame.Navigate(page);
        _currentFrame = frame;

        var item = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is BackstageView view && frame == view);

        if (item != null) _navigationView.SelectedItem = item;
    }

    public void Navigate(BackstageView frame) {
        // This is essentially acting as "select item"
        if (_frame == null || _navigationView == null) return;

        var item = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is BackstageView view && frame == view);

        if (item != null) _navigationView.SelectedItem = item;
    }
}
