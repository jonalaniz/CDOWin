using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;

namespace CDO.Abstractions.Navigation;

public interface INavigationService<TFrame> : INotifyPropertyChanged where TFrame : Enum {
    bool CanNavigate { get; }
    void Initialize(NavigationView navigationView, Frame frame);
    void BackRequested();
    void Navigate(TFrame frame);
}
