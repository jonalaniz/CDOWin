using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDOWin.Services;
using CDOWin.Views.Clients.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class NewRemindersViewModel : ObservableObject {
    private readonly IReminderService _service;
    private readonly int _clientId;

    public DateTime Date;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    public bool CanSave => !string.IsNullOrWhiteSpace(Description);

    public NewRemindersViewModel(IReminderService service, int clientId) {
        _service = service;
        _clientId = clientId;
        Date = DateTime.Now.Date;
    }

    partial void OnDescriptionChanged(string value) {
        Debug.WriteLine("Description Changed");
        OnPropertyChanged(nameof(CanSave));
    }

    public async Task CreateNewReminderAsync() {
        var reminder = new CreateReminderDTO {
            clientID = _clientId,
            date = Date.ToUniversalTime(),
            description = Description
        };
        Debug.WriteLine($"Client ID: {reminder.clientID}");
        Debug.WriteLine($"Date: {reminder.date}");
        Debug.WriteLine($"Description: {reminder.description}");

        await _service.CreateReminderAsync(reminder);
    }

}
