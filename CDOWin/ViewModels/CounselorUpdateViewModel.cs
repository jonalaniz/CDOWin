using CDO.Core.DTOs.Counselors;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CDOWin.ViewModels;

public partial class CounselorUpdateViewModel(CounselorDetail counselor) : ObservableObject {
    public CounselorDetail Original = counselor;
    public CounselorUpdate Updated = new();
}
