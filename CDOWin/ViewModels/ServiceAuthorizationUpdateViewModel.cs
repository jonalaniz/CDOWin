using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationUpdateViewModel(ServiceAuthorization serviceAuthorization) : ObservableObject {
    public ServiceAuthorization Original = serviceAuthorization;
    public UpdateServiceAuthorizationDTO Updated = new();
}
