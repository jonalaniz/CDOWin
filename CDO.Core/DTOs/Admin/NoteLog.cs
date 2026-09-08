namespace CDO.Core.DTOs.Admin;

public record class NoteLog(
    UserSummary? User,
    DateTime CreatedAt,
    DateTime NoteDate,
    string Text
) {
    public string LocalCreatedDate => CreatedAt.ToString(format: "MM/dd/yy");
    public string LocalNoteDate => NoteDate.ToString(format: "MM/dd/yyyy hh:mm tt");
}