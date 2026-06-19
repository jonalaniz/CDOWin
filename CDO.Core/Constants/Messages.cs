namespace CDO.Core.Constants;

public enum MessageType {
    CreatedReminder,
    ExportedClients,
    MarkedBilled,
    MarkedInactive,
    MarkedTTW
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
}