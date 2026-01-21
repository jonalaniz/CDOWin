using CDO.Core.DTOs;
using CDO.Core.Interfaces;
using CDO.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDOWin.Data;

public class DataCoordinator {

    public DataCoordinator(
        IClientService clients,
        ICounselorService counselors,
        IEmployerService employers,
        IPlacementService placements,
        IReminderService reminders,
        IServiceAuthorizationService sas,
        IStateService states,
        DataInvalidationService invalidationService) {
        _clients = clients;
        _counselors = counselors;
        _employers = employers;
        _placements = placements;
        _reminders = reminders;
        _sas = sas;
        _states = states;
        _invalidationService = invalidationService;

        _invalidationService.SAsInvalidated += InvalidateSAs;
        _invalidationService.PlacementsInvalidated += InvalidatePlacements;

    }

    // =========================
    // Services
    // =========================
    private readonly IClientService _clients;
    private readonly ICounselorService _counselors;
    private readonly IEmployerService _employers;
    private readonly IPlacementService _placements;
    private readonly IReminderService _reminders;
    private readonly IServiceAuthorizationService _sas;
    private readonly IStateService _states;
    private readonly DataInvalidationService _invalidationService;

    // =========================
    // Public Fields
    // =========================
    public CachedList<ClientSummaryDTO> Clients { get; } = new();
    public CachedList<Counselor> Counselors { get; } = new();
    public CachedList<Employer> Employers { get; } = new();
    public CachedList<Placement> Placements { get; } = new();
    public CachedList<Reminder> Reminders { get; } = new();
    public CachedList<Invoice> SAs { get; } = new();
    public CachedList<State> States { get; } = new();

    // =========================
    // TTLs
    // =========================
    private static readonly TimeSpan ClientTTL = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan CounselorTTL = TimeSpan.FromHours(30);
    private static readonly TimeSpan EmployerTTL = TimeSpan.FromHours(1);
    private static readonly TimeSpan PlacementTTL = TimeSpan.FromHours(1);
    private static readonly TimeSpan ReminderTTL = TimeSpan.FromSeconds(45);
    private static readonly TimeSpan SATTL = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan StateTTL = TimeSpan.FromDays(365);

    // =========================
    // Invalidation Methods
    // =========================
    private void InvalidatePlacements() => Placements.Invalidate();
    private void InvalidateSAs() => SAs.Invalidate();

    // =========================
    // Update Methods
    // =========================
    public async Task<IReadOnlyList<ClientSummaryDTO>> GetClientsAsync(bool force = false) {
        if (force || Clients.IsStale(ClientTTL)) {
            var data = await _clients.GetAllClientSummariesAsync();
            Clients.Update(data);
        }

        return Clients.Data!;
    }

    public async Task<IReadOnlyList<Counselor>> GetCounselorsAsync(bool force = false) {
        if (force || Counselors.IsStale(CounselorTTL)) {
            var data = await _counselors.GetAllCounselorsAsync();
            Counselors.Update(data);
        }

        return Counselors.Data!;
    }

    public async Task<IReadOnlyList<Employer>> GetEmployersAsync(bool force = false) {
        if (force || Employers.IsStale(EmployerTTL)) {
            var data = await _employers.GetAllEmployersAsync();
            Employers.Update(data);
        }

        return Employers.Data!;
    }

    public async Task<IReadOnlyList<Placement>> GetPlacementsAsync(bool force = false) {
        if (force || Placements.IsStale(PlacementTTL)) {
            var data = await _placements.GetAllPlacementsAsync();
            Placements.Update(data);
        }

        return Placements.Data!;
    }

    public async Task<IReadOnlyList<Reminder>> GetRemindersAsync(bool force = false) {
        if (force || Reminders.IsStale(ReminderTTL)) {
            var data = await _reminders.GetAllRemindersAsync();
            Reminders.Update(data);
        }

        return Reminders.Data!;
    }

    public async Task<IReadOnlyList<Invoice>> GetSAsAsync(bool force = false) {
        if (force || SAs.IsStale(SATTL)) {
            var data = await _sas.GetAllServiceAuthorizationsAsync();
            SAs.Update(data);
        }

        return SAs.Data!;
    }

    public async Task<IReadOnlyList<State>> GetStatesAsync(bool force = false) {
        if (force || States.IsStale(StateTTL)) {
            var data = await _states.GetAllStatesAsync();
            States.Update(data);
        }

        return States.Data!;
    }
}
