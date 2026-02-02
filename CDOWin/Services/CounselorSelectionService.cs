using System;

namespace CDOWin.Services;

public class CounselorSelectionService {
    public event Action<int>? CounselorSelectionRequested;
    public void RequestSelectedCounselor(int counselorId) => CounselorSelectionRequested?.Invoke(counselorId);
}