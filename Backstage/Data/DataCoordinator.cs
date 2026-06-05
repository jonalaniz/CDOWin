using CDO.Core.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.Services.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backstage.Data;

public class DataCoordinator {
    // =========================
    // Services
    // =========================
    private readonly BillingService _billingService;
    private readonly AdminReminderService _reminderService;
    private readonly UserService _userService;

    // =========================
    // Public Fields
    // =========================
    public CachedList<UserSummary> Users { get; } = new();

    // =========================
    // TTLs
    // =========================
    private static readonly TimeSpan BaseTTL = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan UserTTL = TimeSpan.FromMinutes(30);

    // =========================
    // Constructor
    // =========================
    public DataCoordinator(
        BillingService billingService,
        AdminReminderService adminService,
        UserService userService) {
        _billingService = billingService;
        _reminderService = adminService;
        _userService = userService;
    }

    // =========================
    // Update Methods
    // =========================

    // Clients
    //public async Task<IReadOnlyList<AdminClientSummary>?> GetDailyClientSummaries(string? date, bool force = false) {
    //    if (force || )
    //}


    // Users
    public async Task<IReadOnlyList<UserSummary>> GetUsersAsync(bool force = false) {
        if (force || Users.IsStale(UserTTL)) {
            var data = await _userService.GetAllUsersSummariesAsync();
            if (data != null) Users.Update(data);
        }

        return Users.Data ?? [];
    }
}
