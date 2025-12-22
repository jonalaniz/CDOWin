using System.ComponentModel;

namespace CDOWin.Views.Clients;

public class ReminderMenuItem {
    private ReminderMenuItem(string value) { Value = value; }

    public string Value { get; private set; }

    public static ReminderMenuItem NewReminder { get { return new ReminderMenuItem("New Reminder"); } }
    public static ReminderMenuItem FourtyFifthDay { get { return new ReminderMenuItem("45th Day"); } }
    public static ReminderMenuItem NinetiethDay { get { return new ReminderMenuItem("90th Day"); } }
    public override string ToString() {
        return Value;
    }

    public static ReminderMenuItem[] AllItems() {
        return [
            ReminderMenuItem.NewReminder,
            ReminderMenuItem.FourtyFifthDay,
            ReminderMenuItem.NinetiethDay
            ];
    }
}