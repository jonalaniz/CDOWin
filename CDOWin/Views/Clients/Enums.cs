namespace CDOWin.Views.Clients;

public class ReminderMenuItem {
    private ReminderMenuItem(string value, string description, int days) {
        Value = value;
        Description = description;
        Days = days;
    }

    public string Value { get; private set; }
    public string Description { get; private set; }
    public int Days { get; private set; }

    public static ReminderMenuItem FourtyFifthDay {
        get { return new ReminderMenuItem("Create 45th Day Reminder", "Bill 45 Days", 44); }
    }
    public static ReminderMenuItem NinetiethDay {
        get { return new ReminderMenuItem("Create 90th Day Reminder", "Bill 90 Days", 89); }
    }
    public override string ToString() {
        return Value;
    }

    public static ReminderMenuItem[] AllItems() {
        return [
            ReminderMenuItem.FourtyFifthDay,
            ReminderMenuItem.NinetiethDay
        ];
    }
}