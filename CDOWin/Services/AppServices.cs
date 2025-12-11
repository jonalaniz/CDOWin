using CDO.Core.Interfaces;
using CDO.Core.Services;
using CDOWin.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.Services;

public static class AppServices {
    // Network
    public static INetworkService? NetworkService { get; private set; }

    // Services
    public static IClientService? ClientService { get; private set; }
    public static ICounselorService? CounselorService { get; private set; }
    public static IEmployerService? EmployerService { get; private set; }
    public static IServiceAuthorizationService? POsService { get; private set; }
    public static IStateService? StateService { get; private set; }
    public static IReminderService? ReminderService { get; private set; }
    public static IReferralService? ReferralService { get; private set; }

    private static ClientSelectionService _clientSelectionService;

    // ViewModels
    public static ClientsViewModel? ClientsViewModel { get; private set; }
    public static CounselorsViewModel? CounselorsViewModel { get; private set; }
    public static EmployersViewModel? EmployersViewModel { get; private set; }
    public static ServiceAuthorizationsViewModel? POsViewModel { get; private set; }
    public static RemindersViewModel? RemindersViewModel { get; private set; }
    public static StatesViewModel? StatesViewModel { get; private set; }
    public static ReferralsViewModel? ReferralsViewModel { get; private set; }

    // Initialize all services
    public static void InitializeServices(string baseAddress, string apiKey) {
        // Initialize network service
        var network = new NetworkService();
        network.Initialize(baseAddress, apiKey);
        NetworkService = network;

        // Initialize other services
        ClientService = new ClientService(NetworkService);
        CounselorService = new CounselorService(NetworkService);
        EmployerService = new EmployerService(NetworkService);
        POsService = new ServiceAuthorizationService(NetworkService);
        ReminderService = new ReminderService(NetworkService);
        StateService = new StateService(NetworkService);
        ReferralService = new ReferralService(NetworkService);

        _clientSelectionService = new();

        // Initialize ViewModels
        ClientsViewModel = new ClientsViewModel(ClientService, _clientSelectionService);
        CounselorsViewModel = new CounselorsViewModel(CounselorService);
        EmployersViewModel = new EmployersViewModel(EmployerService);
        POsViewModel = new ServiceAuthorizationsViewModel(POsService);
        RemindersViewModel = new RemindersViewModel(ReminderService, _clientSelectionService);
        StatesViewModel = new StatesViewModel(StateService);
        ReferralsViewModel = new ReferralsViewModel(ReferralService);

    }

    public static async Task<bool> LoadDataAsync() {
        var sw = Stopwatch.StartNew();

        var tasks = new List<Task> {
            ClientsViewModel.LoadClientSummariesAsync(),
            CounselorsViewModel.LoadCounselorsAsync(),
            EmployersViewModel.LoadEmployersAsync(),
            POsViewModel.LoadServiceAuthorizationsAsync(),
            RemindersViewModel.LoadRemindersAsync(),
            StatesViewModel.LoadStatesAsync(),
            ReferralsViewModel.LoadReferralsAsync()
        };

        await Task.WhenAll(tasks);
        sw.Stop();
        Debug.WriteLine($"LoadDataAsync completed in {sw.ElapsedMilliseconds}");
        return true;
    }
}
