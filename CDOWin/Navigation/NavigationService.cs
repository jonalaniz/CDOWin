using CDO.Abstractions.Navigation;
using CDOWin.Views;
using CDOWin.Views.Clients;
using CDOWin.Views.Counselors;
using CDOWin.Views.Employers;
using CDOWin.Views.Placements;
using CDOWin.Views.ServiceAuthorizations;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CDOWin.Navigation;

public partial class NavigationService : ObservableObject, INavigationService<CDOFrame> {

    // =========================
    // Dependencies
    // =========================
    private NavigationView? _navigationView;
    private Frame? _frame;

    // =========================
    // State
    // =========================
    private CDOFrame? _currentView;
    private readonly Dictionary<CDOFrame, Type> _pages = new() {
        [CDOFrame.Clients] = typeof(ClientsPage),
        [CDOFrame.Counselors] = typeof(CounselorsPage),
        [CDOFrame.Employers] = typeof(EmployersPage),
        [CDOFrame.ServiceAuthorizations] = typeof(ServiceAuthorizationsPage),
        [CDOFrame.Placements] = typeof(PlacementsPage)
    };
    private readonly ObservableCollection<CDOFrame> _history = [];

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
            args.SelectedItem is not NavigationViewItem { Tag: CDOFrame view } ||
            view == _currentView) return;

        NavigateTo(view);
        if (_currentView is CDOFrame oldFrame) { _history.Add(oldFrame); }
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

        _currentView = view;
        SelectView(view);
    }

    public void RequestNavigation(CDOFrame view) {
        if (_frame == null || _navigationView == null || _currentView == view) return;
        NavigateTo(view);
        SelectView(view);
    }

    // =========================
    // Utility Methods
    // =========================
    private void NavigateTo(CDOFrame view) {
        if (_frame == null || !_pages.TryGetValue(view, out var page)) return;
        _frame.Navigate(page, null, Transition(view));
    }

    private void SelectView(CDOFrame view) {
        if (_navigationView == null) return;
        var item = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is CDOFrame viewItem && viewItem == view);
        if (item != null) _navigationView.SelectedItem = item;
    }

    private SlideNavigationTransitionInfo Transition(CDOFrame newView) {
        if (_currentView is not CDOFrame oldView) return new SlideNavigationTransitionInfo();
        var forward = Comparer<int>.Default.Compare((int)newView, (int)oldView) > 0;

        return forward == true
            ? new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight }
            : new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft };
    }
}
