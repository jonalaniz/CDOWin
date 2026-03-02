using System;

namespace CDOWin.Services;

public class AppSuspensionService {
    public event Action? SuspensionRequested;
    public event Action? ResumeRequested;

    public void RequestSuspension() => SuspensionRequested?.Invoke();
    public void RequetResume() => ResumeRequested?.Invoke();
}
