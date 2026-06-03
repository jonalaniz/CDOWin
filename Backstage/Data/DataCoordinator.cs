using CDO.Core.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backstage.Data; 
public class DataCoordinator {
    // =========================
    // Services
    // =========================
    private readonly IAdminService _adminService;
    private readonly IUserService _userService;

    // =========================
    // Public Fields
    // =========================
    public CachedList<UserSummary> Users { get; } = new();

    // =========================
    // TTLs
    // =========================
    private static readonly TimeSpan UserTTL = TimeSpan.FromMinutes(5);

    // =========================
    // Constructor
    // =========================
    public DataCoordinator(
        IAdminService adminService, 
        IUserService userService ) {
        _adminService = adminService;
        _userService = userService;
    }

    // =========================
    // Update Methods
    // =========================
    public async Task<IReadOnlyList<UserSummary>> GetUsersAsync(bool force = false) {
        if (force || Users.IsStale(UserTTL)) {
            var data = await _userService.GetAllUsersSummariesAsync();
            if (data != null) Users.Update(data);
        }

        return Users.Data ?? [];
    }
}
