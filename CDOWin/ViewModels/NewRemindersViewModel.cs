using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace CDOWin.ViewModels;

public partial class NewRemindersViewModel : ObservableObject {
    private readonly IReminderService _service;
    private DateTime? _date;
    private int _clientId;
    private string? _description;

    public NewRemindersViewModel(IReminderService service, int clientId) {
        _service = service;
        _clientId = clientId;
    }

    public bool CanCreateReminder() {
        // Need a DateTime, Client, and Description
        if (_date == null || string.IsNullOrEmpty(_description))
            return false;
        return true;
    }
}
