using System;

namespace Backstage.Services;

public class UserSelectionService {
    public event Action<string>? UserSelectionRequested;

    public void RequestSelectedUser(string userId) => UserSelectionRequested?.Invoke(userId);
}
