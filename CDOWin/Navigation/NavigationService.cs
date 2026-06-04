using CDO.Abstractions.Navigation;
using CDOWin.Views;
using CDOWin.Views.Clients;
using CDOWin.Views.Counselors;
using CDOWin.Views.Employers;
using CDOWin.Views.Placements;
using CDOWin.Views.ServiceAuthorizations;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDOWin.Navigation;

public sealed class NavigationService : INavigationService<CDOFrame> {
    private NavigationView? _navigationView;
    private Frame? _frame;
    private readonly Dictionary<CDOFrame, Type> _pages = new();
    private CDOFrame? _currentFrame;
    private int _previousSelectedIndex = 0;

    public void Initialize(NavigationView navigationView, Frame frame) {
        _navigationView = navigationView;
        _frame = frame;

        // Seed our page types
        _pages[CDOFrame.Clients] = typeof(ClientsPage);
        _pages[CDOFrame.Counselors] = typeof(CounselorsPage);
        _pages[CDOFrame.Employers] = typeof(EmployersPage);
        _pages[CDOFrame.ServiceAuthorizations] = typeof(ServiceAuthorizationsPage);
        _pages[CDOFrame.Placements] = typeof(PlacementsPage);

        _navigationView.SelectionChanged += SelectionChanged;
    }

    public void Navigate(CDOFrame frame) {
        SelectPage(frame);
    }

    private void ShowPage(CDOFrame frame, Direction direction) {
        if (_frame == null || frame == _currentFrame) return;
        if (_pages.TryGetValue(frame, out var page)) {
            _frame.Navigate(page, null, Transition(direction));
            _currentFrame = frame;
        }
    }

    private void SelectPage(CDOFrame page) {
        if (_frame == null || _navigationView == null) return;

        var item = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is CDOFrame frame && frame == page);

        if (item != null)
            _navigationView.SelectedItem = item;
    }

    private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag is CDOFrame frame) {
            var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
            var direction = currentSelectedIndex - _previousSelectedIndex > 0
                ? Direction.Forward
                : Direction.Backward;
            _previousSelectedIndex = currentSelectedIndex;

            ShowPage(frame, direction);
        }
    }

    private static SlideNavigationTransitionInfo Transition(Direction direction) {
        return direction switch {
            Direction.Forward => new SlideNavigationTransitionInfo {
                Effect = SlideNavigationTransitionEffect.FromRight
            },
            Direction.Backward => new SlideNavigationTransitionInfo {
                Effect = SlideNavigationTransitionEffect.FromLeft
            },
            _ => new SlideNavigationTransitionInfo()
        };
    }
}
