using CDO.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CounselorUpdateViewModel(CounselorResponseDTO counselor) : ObservableObject {
    public CounselorResponseDTO Original = counselor;
    public UpdateCounselorDTO Updated = new();
}
