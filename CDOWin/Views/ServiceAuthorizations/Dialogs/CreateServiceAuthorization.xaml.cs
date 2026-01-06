using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Views.ServiceAuthorizations.Dialogs;

public sealed partial class CreateServiceAuthorization : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreateServiceAuthorizationsViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreateServiceAuthorization(CreateServiceAuthorizationsViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {
        //
    }
}
