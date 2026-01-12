using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;


namespace CDOWin.Views.Placements.Dialogs;

public sealed partial class CreatePlacements : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly CreatePlacementViewModel ViewModel;

    public CreatePlacements(CreatePlacementViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) {

    }

    private void TextChanged(object sender, TextChangedEventArgs e) {

    }

    private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void EndDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void DayOne_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void DayTwo_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void DayThree_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void DayFour_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    private void DayFive_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args) {

    }

    // When the employer is selected, prefill the supervisor/phone/email if the item is empty
}
