using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace CDOWin.ViewModels;

public partial class ClientUpdateViewModel : ObservableObject {
    public Client OriginalClient;
    public UpdateClientDTO UpdatedClient = new UpdateClientDTO();

    public ClientUpdateViewModel(Client client) {
        OriginalClient = client;
    }

    public void UpdateContact() {
        var options = new JsonSerializerOptions {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(UpdatedClient, options);
        Debug.WriteLine(json.ToString());
    }
}