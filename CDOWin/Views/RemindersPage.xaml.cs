using CDO.Core.Models;
using CDOWin.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Text;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CDOWin.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RemindersPage : Page {
    public RemindersViewModel ViewModel { get; }

    public RemindersPage() {
        InitializeComponent();

        ViewModel = AppServices.RemindersViewModel;
        DataContext = ViewModel;
    }

    private void RemindersCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args) {
        // Only operate when phase == 0 (initial render)
        if (args.Phase != 0)
            return;

        DateTime day = args.Item.Date.Date;

        // Does any reminder match this date?
        bool hasReminder = ViewModel.Reminders.Any(r => r.date.Date == day);

        if (hasReminder) {
            // Mark the date (simple highlight)
            args.Item.Background = new SolidColorBrush(Microsoft.UI.Colors.Gold);
            args.Item.FontWeight = FontWeights.Bold;
        } else {
            // Reset to defaults when not a reminder date
            args.Item.Background = null;
            args.Item.FontWeight = FontWeights.Normal;
        }
    }
}
