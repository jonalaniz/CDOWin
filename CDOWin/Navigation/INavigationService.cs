using CDOWin.Views;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Navigation;

public interface INavigationService {
    event Action<CDOFrame>? NavigationRequested;
    void Initialize(NavigationView navigationView, Frame frame);
    void Navigate(CDOFrame frame);
}
