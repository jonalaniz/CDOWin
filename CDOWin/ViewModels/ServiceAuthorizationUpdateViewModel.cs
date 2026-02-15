using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.Interfaces;
using CDOWin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationUpdateViewModel(InvoiceDetail serviceAuthorization) : ObservableObject {
    private IServiceAuthorizationService _service = AppServices.SAService;
    public InvoiceDetail Original = serviceAuthorization;
    public SAUpdate Updated = new();

    public async Task<Result> UpdateSAAsync() {
        return await _service.UpdateServiceAuthorizationAsync(Original.Id, Updated);
    }
}
