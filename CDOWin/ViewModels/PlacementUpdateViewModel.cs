using CDO.Core.DTOs.Placements;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class PlacementUpdateViewModel(PlacementDetail placement) : ObservableObject {
    private readonly IPlacementService _service = AppServices.PlacementService;
    public PlacementDetail Original = placement;
    public PlacementUpdate Updated = new();

    public async Task<Result> UpdatePlacementAsync() {
        return await _service.UpdatePlacementAsync(Original.Id, Updated);
    }
}
