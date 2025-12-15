using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.ViewModels;

public partial class ReminderUpdateViewModel : ObservableObject {
    public Reminder Original;
    public UpdateReminderDTO Updated = new UpdateReminderDTO();

    public ReminderUpdateViewModel(Reminder reminder) {
        Original = reminder;
    }
}
