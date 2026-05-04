using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class CreateNote : Page {

    // =========================
    // Dependencies
    // =========================
    public CreateNoteViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public CreateNote(CreateNoteViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupDateAndTime();
    }

    private void SetupDateAndTime() {
        var date = DateTime.Now;
        DatePicker.Date = date;
        TimePicker.Time = date.TimeOfDay;
    }

    // =========================
    // Property Change Methods
    // =========================
    private void LabeledMultiLinePair_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox)
            return;

        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text))
            return;

        ViewModel.Note = text;
    }

    private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e) {
        UpdateDateAndTime();
    }

    private void TimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e) {
        UpdateDateAndTime();
    }

    private void UpdateDateAndTime() {
        if (TimePicker.Time is TimeSpan timeSpan && DatePicker.Date is DateTimeOffset offset) {
            ViewModel.Date = new DateTime(
                    offset.Year,
                    offset.Month,
                    offset.Day,
                    timeSpan.Hours,
                    timeSpan.Minutes,
                    timeSpan.Seconds,
                    DateTimeKind.Local
                    ).ToUniversalTime();
        }
    }
}
