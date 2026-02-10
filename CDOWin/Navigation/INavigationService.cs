using CDOWin.Views;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Navigation;

public interface INavigationService {
    void Initialize(NavigationView navigationView, Frame frame);
    void Navigate(CDOFrame frame);
}
