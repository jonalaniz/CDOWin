using CDO.Core.DTOs.Admin;
using Microsoft.UI.Xaml.Controls;

namespace Backstage.Dialogs; 
public sealed partial class ReminderDetailPage : Page {

    // =========================
    // Dependencies
    // =========================
    private AdminReminderDetail Reminder;

    // =========================
    // Constructor
    // =========================
    public ReminderDetailPage(AdminReminderDetail reminder) {
        Reminder = reminder;
        InitializeComponent();
    }
}
