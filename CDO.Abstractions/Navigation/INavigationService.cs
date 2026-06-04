using Microsoft.UI.Xaml.Controls;
using System;

namespace CDO.Abstractions.Navigation;

public interface INavigationService<TFrame> where TFrame : Enum {
    void Initialize(NavigationView navigationView, Frame frame);
    void Navigate(TFrame frame);
}
