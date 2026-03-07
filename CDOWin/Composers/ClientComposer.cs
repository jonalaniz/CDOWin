using CDO.Core.DTOs.Clients;
using CDO.Core.ErrorHandling;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace CDOWin.Composers;

public sealed class ClientComposer {
    private readonly Serializer _serializer = new();

    public Result ComposeClientToFile(ClientDetail client) {
        // Check and create path
        if (client.DocumentsFolderPath is not string path)
            return Result.Fail(new AppError(ErrorKind.Unknown, "Missing file path.", null));

        if (!Directory.Exists(client.DocumentsFolderPath))
            return Result.Fail(new AppError(ErrorKind.Unknown, "File path does not exist", null));

        var filePath = Path.Combine(path, "Client.txt");

        // Export
        var clientExport = client.AsExport();
        var yaml = _serializer.Serialize(clientExport);
        
        // Write to disk
        try {
            File.WriteAllText(filePath, yaml, Encoding.UTF8);
        } catch {
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to write file to disk.", null));
        }

        return Result.Success();
    }
}
