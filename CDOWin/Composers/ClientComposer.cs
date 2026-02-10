using CDO.Core.DTOs.Clients;
using CDO.Core.ErrorHandling;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace CDOWin.Composers;

public sealed class ClientComposer {
    private readonly Serializer _serializer = new();

    public Result ComposeClientToFile(ClientDetail client) {
        if (client.DocumentsFolderPath == null)
            return Result.Fail(new AppError(ErrorKind.Unknown, "Missing file path.", null));

        var clientExport = client.AsExport();
        var yaml = _serializer.Serialize(clientExport);
        var filePath = Path.Combine(client.DocumentsFolderPath, "ClientDetail.txt");

        try {
            File.WriteAllText(filePath, yaml, Encoding.UTF8);
        } catch {
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to write file to disk.", null));
        }

        return Result.Success();
    }
}
