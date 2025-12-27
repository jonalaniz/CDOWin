namespace CDOWin.Views.Clients;

public class ReminderMenuItem {
    private ReminderMenuItem(string value, string description) { 
        Value = value;
        Description = description;
    }

    public string Value { get; private set; }
    public string Description { get; private set; }

    public static ReminderMenuItem FourtyFifthDay { 
        get { return new ReminderMenuItem("45th Day Reminder", "Bill 45 Days"); }
    }
    public static ReminderMenuItem NinetiethDay { 
        get { return new ReminderMenuItem("90th Day Reminder", "Bill 90 Days"); } 
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