using CDO.Core.DTOs.Clients;
using CDOWin.Views.Clients.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ClientUpdateViewModel : ObservableObject {
    // =========================
    // Dependencies
    // =========================
    public ClientDetail OriginalClient;
    public ClientUpdate UpdatedClient = new();

    [ObservableProperty]
    public partial string? FolderPath { get; set; }

    public ClientUpdateViewModel(ClientDetail client) {
        OriginalClient = client;
        FolderPath = client.DocumentsFolderPath;
    }

    // =========================
    // CRUD Methods
    // =========================
    public void UpdateCheckbox(CheckboxTag tag, bool isChecked) {
        switch (tag) {
            case CheckboxTag.ResumeRequired:
                UpdatedClient.ResumeRequired = isChecked;
                break;
            case CheckboxTag.ResumeCompleted:
                UpdatedClient.ResumeCompleted = isChecked;
                break;
            case CheckboxTag.VideoInterviewRequired:
                UpdatedClient.VideoInterviewRequired = isChecked;
                break;
            case CheckboxTag.VideoInterviewCompleted:
                UpdatedClient.VideoInterviewCompleted = isChecked;
                break;
            case CheckboxTag.ReleasesCompleted:
                UpdatedClient.ReleasesCompleted = isChecked;
                break;
            case CheckboxTag.OrientationCompleted:
                UpdatedClient.OrientationCompleted = isChecked;
                break;
            case CheckboxTag.DataSheetCompleted:
                UpdatedClient.DataSheetCompleted = isChecked;
                break;
            case CheckboxTag.ElevatorSpeechCompleted:
                UpdatedClient.ElevatorSpeechCompleted = isChecked;
                break;
        }
    }
}