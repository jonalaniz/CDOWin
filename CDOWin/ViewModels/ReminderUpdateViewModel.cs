using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ReminderUpdateViewModel(Reminder reminder) : ObservableObject {
    public Reminder Original = reminder;
    public UpdateReminderDTO Updated = new();
}
