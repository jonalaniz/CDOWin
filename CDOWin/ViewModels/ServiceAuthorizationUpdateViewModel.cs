using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationUpdateViewModel(ServiceAuthorization serviceAuthorization) : ObservableObject {
    private IServiceAuthorizationService _service = AppServices.SAService;
    public ServiceAuthorization Original = serviceAuthorization;
    public UpdateServiceAuthorizationDTO Updated = new();

    public async Task<Result<ServiceAuthorization>> UpdateSAAsync() {
        return await _service.UpdateServiceAuthorizationAsync(Original.Id, Updated);
    }
}
