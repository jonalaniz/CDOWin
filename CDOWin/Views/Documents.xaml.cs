using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace CDOWin.Views;

public sealed partial class Documents : Page, INotifyPropertyChanged {
    public ObservableCollection<string> Files { get; private set; } = [];

    public event PropertyChangedEventHandler PropertyChanged;

    public ClientsViewModel ViewModel { get; private set; }

    public Documents() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        ViewModel = (ClientsViewModel)e.Parameter;
        DataContext = ViewModel;

        // Subscribe to changes
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        LoadFiles();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
        if (ViewModel != null) {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(ViewModel.SelectedClient)) {
            LoadFiles();
        }
    }

    public void LoadFiles() {
        var path = ViewModel.SelectedClient?.documentsFolderPath;

        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) {
            Files.Clear();
            return;
        }

        Files.Clear();

        foreach (var file in Directory.GetFiles(path)) {
            Files.Add(Path.GetFileName(file));
        }
    }
}
