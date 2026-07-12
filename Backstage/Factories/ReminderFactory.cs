using CDO.Core.DTOs.Reminders;
using System;

namespace Backstage.Factories;

public enum ReminderDate {
    Today,
    Tomorrow
}

public enum ClientReminderType {
    StaleClient
}

public enum SAReminderType {
    StaleSA,
    UnbilledSA
}

public static class ReminderFactory {
    public static NewReminder CreateClientReminder(int clientId, ReminderDate date) {
        return new NewReminder(
            ClientID: clientId, 
            Date: Date(date), 
            Description: Description(ClientReminderType.StaleClient)
            );
    }

    public static NewReminder CreateSAReminder(int clientId, ReminderDate date, string saNumber, SAReminderType type) {
        return new NewReminder(
            ClientID: clientId, 
            Date: Date(date), 
            Description: Description(type, saNumber)
            );
    }

    private static DateTime Date(ReminderDate reminderDate) {
        return reminderDate switch {
            ReminderDate.Today => DateTime.Now.Date,
            ReminderDate.Tomorrow => DateTime.Now.Date.AddDays(1),
            _ => throw new ArgumentException(nameof(reminderDate))
        };
    }

    private static string Description(ClientReminderType type) {
        return type switch {
            ClientReminderType.StaleClient => "Check in on client",
            _ => throw new ArgumentException(nameof(type))
        };
    }

    private static string Description(SAReminderType type, string saId) {
        return type switch {
            SAReminderType.StaleSA => $"Check expiration status for SA: {saId}",
            SAReminderType.UnbilledSA => $"Check billing status for SA: {saId}",
            _ => throw new ArgumentException(nameof(type))
        };
    }
}