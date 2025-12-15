using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.ViewModels;

public partial class ServiceAuthorizationUpdateViewModel : ObservableObject {
    public ServiceAuthorization Original;
    public UpdateServiceAuthorizationDTO Updated = new UpdateServiceAuthorizationDTO();

    public ServiceAuthorizationUpdateViewModel(ServiceAuthorization serviceAuthorization) {
        Original = serviceAuthorization;
    }
}
