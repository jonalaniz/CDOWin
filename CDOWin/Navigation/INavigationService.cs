using CDOWin.Views;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Navigation;

public interface INavigationService {
    event Action<CDOFrame>? NavigationRequested;
    void SetFrame(Frame frame);
    void Navigate(CDOFrame frame);
    void ShowClients(Direction direction);
    void ShowCounselors(Direction direction);
    void ShowEmployers(Direction direction);
    void ShowPlacements(Direction direction);
    void ShowServiceAuthorizations(Direction direction);
}
