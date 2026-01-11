using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Views.Clients.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ClientUpdateViewModel(Client client) : ObservableObject {
    // =========================
    // Dependencies
    // =========================
    public Client OriginalClient = client;
    public UpdateClientDTO UpdatedClient = new();

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