using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class PlacementUpdateViewModel(Placement placement) : ObservableObject {
    public Placement Original = placement;
    public PlacementDTO Updated = new();
}
