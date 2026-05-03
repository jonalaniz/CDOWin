using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.ErrorHandling;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace CDOWin.Composers;

public sealed class NotesComposer {
    private readonly Serializer _serializer = new();

    public Result ComposeNotesToFile(ClientNote[] notes) {
        // Get Temp Path
        var path = Path.Combine(Path.GetTempPath(), "Notes.txt");

        // Format Notes
        var formattedNotes = notes.Select(n => n.AsFormattedNote()).ToArray();

        // Serialize Notes
        var yaml = _serializer.Serialize(formattedNotes);

        // Write to disk
        try {
            File.WriteAllText(path, yaml, Encoding.UTF8);
            Process.Start("notepad.exe", path);
        } catch {
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to write notes to disk.", null));
        }

        return Result.Success();
    }
}