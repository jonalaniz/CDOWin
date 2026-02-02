using System;

namespace CDOWin.Services;

public class EmployerSelectionService {
    public event Action<int>? EmployerSelectionRequested;
    public void RequestSelectedEmployer(int employerId) => EmployerSelectionRequested?.Invoke(employerId);
}
