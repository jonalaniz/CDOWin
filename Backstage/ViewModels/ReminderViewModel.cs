using CDO.Core.DTOs.Reminders;
using CDO.Core.ErrorHandling;
using CDO.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;

namespace Backstage.ViewModels;

public partial class ReminderViewModel(ReminderService reminderService) : ObservableObject {

    // =========================
    // Dependencies
    // =========================
    private readonly ReminderService _service = reminderService;

    // =========================
    // Post Methods
    // =========================
    public async Task<Result<ReminderDetail>> CreateReminderAsync(NewReminder reminder) {
        return await _service.CreateRemindersAsync(reminder);
    }
}
