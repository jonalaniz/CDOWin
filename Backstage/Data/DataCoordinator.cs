using CDO.Core.Data;
using CDO.Core.DTOs.Admin;
using CDO.Core.DTOs.Clients.Notes;
using CDO.Core.DTOs.Placements;
using CDO.Core.DTOs.SAs;
using CDO.Core.Services.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backstage.Data;

public class DataCoordinator {
    // =========================
    // Services
    // =========================
    private readonly BillingService _billingService;
    private readonly AdminClientService _clientService;
    private readonly AdminReminderService _reminderService;
    private readonly UserService _userService;

    // =========================
    // Public Fields
    // =========================
    public CachedList<SASummary> UnbilledSAs { get; } = new();
    public CachedList<PlacementSummary> UnbilledPlacements { get; } = new();
    public CachedList<AdminClientSummary> RecentClients { get; } = new();
    public CachedList<ClientNote> RecentNotes { get; } = new();
    public CachedList<AdminClientSummary> StaleClients { get; } = new();
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
        AdminClientService clientService,
        AdminReminderService reminderService,
        UserService userService) {
        _billingService = billingService;
        _clientService = clientService;
        _reminderService = reminderService;
        _userService = userService;
    }

    // =========================
    // Update Methods
    // =========================

    // Billing

    public async Task<IReadOnlyList<SASummary>> GetUnbilledSAsAsync(bool force = false) {
        if (force || UnbilledSAs.IsStale(BaseTTL)) {
            var data = await _billingService.GetUnbilledSAsAsync();
            if (data != null) UnbilledSAs.Update(data);
        }

        return UnbilledSAs.Data ?? [];
    }

    public async Task<IReadOnlyList<PlacementSummary>> GetUnbilledPlacementsAsync(bool force = false) {
        if (force || UnbilledPlacements.IsStale(BaseTTL)) {
            var data = await _billingService.GetUnbilledPlacementsAsync();
            if (data != null) UnbilledPlacements.Update(data);
        }

        return UnbilledPlacements.Data ?? [];
    }

    // Clients

    public async Task<IReadOnlyList<AdminClientSummary>> GetRecentClientsAsync(bool force = false) {
        if (force || RecentClients.IsStale(BaseTTL)) {
            var data = await _clientService.GetRecentClientSummariesAsync(date: null);
            if (data != null) RecentClients.Update(data);
        }

        return RecentClients.Data ?? [];
    }

    public async Task<IReadOnlyList<AdminClientSummary>> GetStaleClientsAsync(bool force = false) {
        if (force || StaleClients.IsStale(BaseTTL)) {
            var data = await _clientService.GetStaleClientsAsync();
            if (data != null) StaleClients.Update(data);
        }

        return StaleClients.Data ?? [];
    }

    public async Task<IReadOnlyList<ClientNote>> GetRecentNotesAsync(bool force = false) {
        if (force || RecentNotes.IsStale(BaseTTL)) {
            var data = await _clientService.GetRecentClientNotesAsync();
            if (data != null) RecentNotes.Update(data);
        }

        return RecentNotes.Data ?? [];
    }

    // Users
    public async Task<IReadOnlyList<UserSummary>> GetUsersAsync(bool force = false) {
        if (force || Users.IsStale(UserTTL)) {
            var data = await _userService.GetAllUsersSummariesAsync();
            if (data != null) Users.Update(data);
        }

        return Users.Data ?? [];
    }
}
