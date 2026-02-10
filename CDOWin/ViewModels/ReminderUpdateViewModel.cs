using CDO.Core.DTOs.Reminders;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ReminderUpdateViewModel(Reminder reminder) : ObservableObject {
    public Reminder Original = reminder;
    public ReminderUpdate Updated = new();
}
