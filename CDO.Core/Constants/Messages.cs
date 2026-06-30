namespace CDO.Core.Constants;

public enum MessageType {
    CreatedReminder,
    ExportedClients,
    MarkedBilled,
    MarkedInactive,
    MarkedTTW
}

public enum ClientPageMessageType {
    MarkedTTW,
    UnmarkedTTW,
    MarkedActive,
    MarkedInactive,
    UpdatedClient,
    DeletedClient,
    CreatedReminder,
    CreatedSA,
    UpdatedSA,
    ExportedSA,
    CreatedPlacement,
    UpdatedPlacement,
    DocumentsFolderMissing
}

public static class Messages {
    public static string MessageForType(MessageType type, bool success) => (type, success) switch {
        (MessageType.CreatedReminder, true) => "Successfully created reminder.",
        (MessageType.CreatedReminder, false) => "Unable to create reminder.",
        (MessageType.ExportedClients, true) => "Successfully exported Clients.",
        (MessageType.ExportedClients, false) => "Unable to export Clients.",
        (MessageType.MarkedBilled, true) => "Successfully marked SA as billed.",
        (MessageType.MarkedBilled, false) => "Unable to mark SA as billed.",
        (MessageType.MarkedInactive, true) => "Successfully marked Client inactive.",
        (MessageType.MarkedInactive, false) => "Unable to mark Client as inactive.",
        (MessageType.MarkedTTW, true) => "Successfully marked Client as TTW.",
        (MessageType.MarkedTTW, false) => "Unable to mark Client as TTW.",
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static string MessageForType(ClientPageMessageType type, bool success) => (type, success) switch {
        (ClientPageMessageType.MarkedTTW, true) => "Successfully marked Client as TTW",
        (ClientPageMessageType.MarkedTTW, false) => "Unable to mark Client as TTW",
        (ClientPageMessageType.UnmarkedTTW, true) => "Successfully unmarked Client as TTW",
        (ClientPageMessageType.UnmarkedTTW, false) => "Unable to unmark Client as TTW",
        (ClientPageMessageType.MarkedActive, true) => "Successfully marked Client as active.",
        (ClientPageMessageType.MarkedActive, false) => "Unable to mark Client as active.",
        (ClientPageMessageType.MarkedInactive, true) => "Successfully marked Client inactive.",
        (ClientPageMessageType.MarkedInactive, false) => "Unable to mark Client inactive.",
        (ClientPageMessageType.UpdatedClient, true) => "Successfully updated Client.",
        (ClientPageMessageType.UpdatedClient, false) => "Unable to update Client.",
        (ClientPageMessageType.DeletedClient, true) => "Successfully deleted Client.",
        (ClientPageMessageType.DeletedClient, false) => "Unable to delete Client.",
        (ClientPageMessageType.CreatedReminder, true) => "Successfully created Reminder.",
        (ClientPageMessageType.CreatedReminder, false) => "Unable to create Reminder.",
        (ClientPageMessageType.CreatedSA, true) => "Successfully created Service Authorization.",
        (ClientPageMessageType.CreatedSA, false) => "Unable to create Service Authorization.",
        (ClientPageMessageType.UpdatedSA, true) => "Successfully updated Service Authorization.",
        (ClientPageMessageType.UpdatedSA, false) => "Unable to update Service Authorization.",
        (ClientPageMessageType.ExportedSA, true) => "Successfully exported Service Authorization.",
        (ClientPageMessageType.ExportedSA, false) => "Unable to export Service Authorization.",
        (ClientPageMessageType.CreatedPlacement, true) => "Successfully created Placement.",
        (ClientPageMessageType.CreatedPlacement, false) => "Unable to create Placement.",
        (ClientPageMessageType.UpdatedPlacement, true) => "Successfully updated Placement.",
        (ClientPageMessageType.UpdatedPlacement, false) => "Unable to update Placement.",
        (ClientPageMessageType.DocumentsFolderMissing, true) => "Documents folder missing, check shared drive.",
        (ClientPageMessageType.DocumentsFolderMissing, false) => "Documents folder missing, check shared drive.",
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}

