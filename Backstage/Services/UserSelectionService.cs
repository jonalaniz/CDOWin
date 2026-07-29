using System;
using System.Collections.Generic;
using System.Text;

namespace Backstage.Services; 
public class UserSelectionService {
    public event Action<string>? UserSelectionRequested;

    public void RequestSelectedUser(string userId) => UserSelectionRequested?.Invoke(userId);
}
