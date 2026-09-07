using CDO.Core.DTOs.Reminders;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ReminderUpdateViewModel(ReminderDetail reminder) : ObservableObject {
    public ReminderDetail Original = reminder;
    public ReminderUpdate Updated = new();
}
