using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class PlacementUpdateViewModel(Placement placement) : ObservableObject {
    private IPlacementService _service = AppServices.PlacementService;
    public Placement Original = placement;
    public PlacementDTO Updated = new();

    public async Task<Result<Placement>> UpdatePlacementAsync() {
        return await _service.UpdatePlacementAsync(Original.Id, Updated);
    }
}
