using CDO.Core.DTOs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationUpdateViewModel(Invoice serviceAuthorization) : ObservableObject {
    private IServiceAuthorizationService _service = AppServices.SAService;
    public Invoice Original = serviceAuthorization;
    public UpdateInvoiceDTO Updated = new();

    public async Task<Result<Invoice>> UpdateSAAsync() {
        return await _service.UpdateServiceAuthorizationAsync(Original.ServiceAuthorizationNumber, Updated);
    }
}
