using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.ViewModels;
    public partial class EmployerUpdateViewModel : ObservableObject {
    public Employer Original;
    public EmployerDTO Updated = new EmployerDTO();

    public EmployerUpdateViewModel(Employer employer) {
        Original = employer;
    }
    }
