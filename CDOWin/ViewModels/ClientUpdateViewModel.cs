using CDO.Core.DTOs;
using CDO.Core.Models;
using CDOWin.Views.Clients.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ClientUpdateViewModel : ObservableObject {
    public Client OriginalClient;
    public UpdateClientDTO UpdatedClient = new UpdateClientDTO();

    public ClientUpdateViewModel(Client client) {
        OriginalClient = client;
    }

    public void UpdateCheckbox(CheckboxTag tag, bool isChecked) {
        switch (tag) {
            case CheckboxTag.ResumeRequired:
                UpdatedClient.resumeRequired = isChecked;
                break;
            case CheckboxTag.ResumeCompleted:
                UpdatedClient.resumeCompleted = isChecked;
                break;
            case CheckboxTag.VideoInterviewRequired:
                UpdatedClient.videoInterviewRequired = isChecked;
                break;
            case CheckboxTag.VideoInterviewCompleted:
                UpdatedClient.videoInterviewCompleted = isChecked;
                break;
            case CheckboxTag.ReleasesCompleted:
                UpdatedClient.releasesCompleted = isChecked;
                break;
            case CheckboxTag.OrientationCompleted:
                UpdatedClient.orientationCompleted = isChecked;
                break;
            case CheckboxTag.DataSheetCompleted:
                UpdatedClient.dataSheetCompleted = isChecked;
                break;
            case CheckboxTag.ElevatorSpeechCompleted:
                UpdatedClient.elevatorSpeechCompleted = isChecked;
                break;
        }
    }
}