using CDOWin.Views;
using CDOWin.Views.Clients;
using CDOWin.Views.Counselors;
using CDOWin.Views.Employers;
using CDOWin.Views.Placements;
using CDOWin.Views.ServiceAuthorizations;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace CDOWin.Navigation;

public sealed class NavigationService : INavigationService {
    private Frame? _frame;

    public event Action<CDOFrame>? NavigationRequested;

    public void SetFrame(Frame frame) => _frame = frame;

    public void Navigate(CDOFrame frame) {
        NavigationRequested?.Invoke(frame);
    }

    public void ShowClients(Direction direction) {
        _frame?.Navigate(typeof(ClientsPage), null, Transition(direction));
    }

    public void ShowCounselors(Direction direction) {
        _frame?.Navigate(typeof(CounselorsPage), null, Transition(direction));
    }

    public void ShowEmployers(Direction direction) {
        _frame?.Navigate(typeof(EmployersPage), null, Transition(direction));
    }

    public void ShowPlacements(Direction direction) {
        _frame?.Navigate(typeof(PlacementsPage), null, Transition(direction));
    }

    public void ShowServiceAuthorizations(Direction direction) {
        _frame?.Navigate(typeof(ServiceAuthorizationsPage), null, Transition(direction));
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
