using CDO.Core.ErrorHandling;
using CDO.Core.Models;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace CDOWin.Composers;

public sealed class ClientComposer {
    private readonly Serializer _serializer = new();

    public Result ComposeClientToFile(Client client) {
        if (client.DocumentsFolderPath == null)
            return Result.Fail(new AppError(ErrorKind.Unknown, "Missing file path.", null));

        var clientSummary = client.AsSummary();
        var yaml = _serializer.Serialize(clientSummary);
        var filePath = Path.Combine(client.DocumentsFolderPath, "Client.txt");

        try {
            File.WriteAllText(filePath, yaml, Encoding.UTF8);
        } catch {
            return Result.Fail(new AppError(ErrorKind.Unknown, "Unable to write file to disk.", null));
        }

        return Result.Success();
    }
}
