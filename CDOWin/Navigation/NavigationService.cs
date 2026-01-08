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

public sealed class NavigationService : INavigationService {
    private NavigationView? _navigationView;
    private Frame? _frame;
    private readonly Dictionary<CDOFrame, Page> _pages = new();
    private CDOFrame? _currentFrame;
    private int _previousSelectedIndex = 0;

    public event Action<CDOFrame>? NavigationRequested;

    public void Initialize(NavigationView navigationView, Frame frame) {
        _navigationView = navigationView;
        _frame = frame;

        // Preload our pages
        _pages[CDOFrame.Clients] = new ClientsPage();
        _pages[CDOFrame.Counselors] = new CounselorsPage();
        _pages[CDOFrame.Employers] = new EmployersPage();
        _pages[CDOFrame.ServiceAuthorizations] = new ServiceAuthorizationsPage();
        _pages[CDOFrame.Placements] = new PlacementsPage();

        _navigationView.SelectionChanged += SelectionChanged;
    }

    public void Navigate(CDOFrame frame) {
        SelectPage(frame);
    }

    private void ShowPage(CDOFrame frame, Direction direction) {
        if (_frame == null || frame == _currentFrame) return;
        if (_pages.TryGetValue(frame, out var page)) {
            _frame.Navigate(page.GetType(), null, Transition(direction));
            _currentFrame = frame;
        }
    }

    private void SelectPage(CDOFrame page) {
        if (_frame == null || _navigationView == null) return;

        foreach (var item in _navigationView.MenuItems) {
            if (item is NavigationViewItem nvi && nvi.Tag is CDOFrame frame && frame == page) {
                _navigationView.SelectedItem = nvi;
                return;
            }
        }
    }

    private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag is CDOFrame frame) {
            var currentSelectedIndex = sender.MenuItems.IndexOf(selectedItem);
            var direction = currentSelectedIndex - _previousSelectedIndex > 0
                ? Direction.Forward
                : Direction.Backward;
            _previousSelectedIndex = currentSelectedIndex;

            switch (frame) {
                case CDOFrame.Clients:
                    ShowPage(CDOFrame.Clients, direction);
                    break;
                case CDOFrame.Counselors:
                    ShowPage(CDOFrame.Counselors, direction);
                    break;
                case CDOFrame.Employers:
                    ShowPage(CDOFrame.Employers, direction);
                    break;
                case CDOFrame.ServiceAuthorizations:
                    ShowPage(CDOFrame.ServiceAuthorizations, direction);
                    break;
                case CDOFrame.Placements:
                    ShowPage(CDOFrame.Placements, direction);
                    break;
            }
            ;
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
