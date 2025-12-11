using CDOWin.ViewModels;
using CDOWin.Views.Employers.Inspectors;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace CDOWin.Views;

public sealed partial class EmployersPage : Page {
    public EmployersViewModel ViewModel { get; }

    public EmployersPage() {
        InitializeComponent();
        ViewModel = AppServices.EmployersViewModel;
        DataContext = ViewModel;
        InspectorFrame.Navigate(typeof(EmployerInspector), ViewModel);
    }
}
