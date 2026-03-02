using CDOWin.Services;
using Microsoft.Windows.System.Power;

namespace CDOWin.Observers;

public sealed class PowerStateObserver {
    private readonly AppSuspensionService _appSuspensionService;

    public PowerStateObserver(AppSuspensionService service) {
        _appSuspensionService = service;
        PowerManager.SystemSuspendStatusChanged += OnSuspendStatusChanged;
    }

    private void OnSuspendStatusChanged(object? sender, object e) {
        var status = PowerManager.SystemSuspendStatus;

        if (status == SystemSuspendStatus.Entering) {
            _appSuspensionService.RequestSuspension();
        } else if (status == SystemSuspendStatus.ManualResume || status == SystemSuspendStatus.AutoResume) {
            _appSuspensionService.RequetResume();
        }
    }
}
