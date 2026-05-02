using CDO.Core.DTOs.Clients.Notes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class NoteUpdateViewModel(ClientNote note) : ObservableObject {
    public ClientNote Original = note;
    public NoteUpdate Updated = new();
}
