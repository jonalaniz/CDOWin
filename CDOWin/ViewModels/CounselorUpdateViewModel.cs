using CDO.Core.DTOs;
using CDO.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CounselorUpdateViewModel(Counselor counselor) : ObservableObject {
    public Counselor Original = counselor;
    public UpdateCounselorDTO Updated = new();
}
