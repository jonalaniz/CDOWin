using CDO.Core.Models;
using CDOWin.ViewModels;
using Microsoft.UI;
using Microsoft.UI.System;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.UI;

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
        // Render basic day items.
        if (args.Phase == 0) {
            // Register callback for next phase.
            args.RegisterUpdateCallback(RemindersCalendar_CalendarViewDayItemChanging);
        } else if (args.Phase == 1) {
            DateTime day = args.Item.Date.Date;

            // Does any reminder match this date?
            bool hasReminder = ViewModel.Reminders.Any(r => r.date.Date == day);

            if (hasReminder) {
                // Mark the date (simple highlight)
                Color accentColor = (Color)Application.Current.Resources["SystemAccentColorDark2"];
                args.Item.Background = new SolidColorBrush(accentColor);
                args.Item.FontWeight = FontWeights.Bold;
            } else {
                // Reset to defaults when not a reminder date
                args.Item.Background = null;
                args.Item.FontWeight = FontWeights.Normal;
            }
        }
    }
}
