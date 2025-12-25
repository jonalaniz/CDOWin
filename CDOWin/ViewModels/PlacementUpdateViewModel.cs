using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class PlacementUpdateViewModel : ObservableObject {
    public Placement Original;
    public PlacementDTO Updated = new PlacementDTO();

    public PlacementUpdateViewModel(Placement placement) {
        Original = placement;
    }
}
