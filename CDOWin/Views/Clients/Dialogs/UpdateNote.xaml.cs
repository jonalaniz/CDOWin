using CDOWin.Extensions;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Views.Clients.Dialogs;

public sealed partial class UpdateNote : Page {

    // =========================
    // Dependencies
    // =========================
    public NoteUpdateViewModel ViewModel;

    // =========================
    // Constructor
    // =========================
    public UpdateNote(NoteUpdateViewModel viewModel) {
        ViewModel = viewModel;
        InitializeComponent();
        SetupTimeAndDatePickers();
    }


    // =========================
    // UI Setup
    // =========================
    private void SetupTimeAndDatePickers() {
        var localDate = LocalOriginalTime();

        DatePicker.Date = new DateTimeOffset(localDate);
        TimePicker.Time = localDate.TimeOfDay;
    }

    // =========================
    // Property Change Methods
    // =========================
    private void Note_TextChanged(object sender, TextChangedEventArgs e) {
        if (sender is not TextBox textbox) return;
        var text = textbox.Text.NormalizeString();

        if (string.IsNullOrWhiteSpace(text)) return;
        ViewModel.Updated.Note = text;
    }

    private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e) {
        if (DatePicker.Date is DateTimeOffset offset &&
            new DateTimeOffset(LocalOriginalTime()).Date == offset.Date)
            return;

        UpdateDateAndTime();
    }

    private void TimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e) {
        if (LocalOriginalTime().TimeOfDay == TimePicker.Time) return;

        UpdateDateAndTime();
    }

    private DateTime LocalOriginalTime() {
        return TimeZoneInfo.ConvertTimeFromUtc(ViewModel.Original.Date, TimeZoneInfo.Local);
    }

    private void UpdateDateAndTime() {
        if (TimePicker.Time is TimeSpan timeSpan && DatePicker.Date is DateTimeOffset offset) {
            ViewModel.Updated.Date = new DateTime(
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
