using Backstage.Views;
using CDO.Abstractions.Navigation;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace Backstage.Navigation;

public sealed class NavigationService : INavigationService<BackstageView> {
    private NavigationView? _navigationView;
    private Frame? _frame;
    private readonly Dictionary<BackstageView, Type> _pages = new();
    private BackstageView? _currentFrame;

    public void Initialize(NavigationView navigationView, Frame frame) {
        _navigationView = navigationView;
        _frame = frame;

        // Seed our page types
        _pages[BackstageView.Home] = typeof(AdminPage);
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
        _currentFrame = frame;
    }

    // To be added at a future date
    public void Navigate(BackstageView frame) { }
}
