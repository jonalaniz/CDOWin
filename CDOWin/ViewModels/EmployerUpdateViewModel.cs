using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class EmployerUpdateViewModel(Employer employer) : ObservableObject {
    public Employer Original = employer;
    public EmployerDTO Updated = new();
}
