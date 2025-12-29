using CDO.Core.Interfaces;
using CDO.Core.Services;
using CDOWin.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.Services;

public static class AppServices {
    // Network
    public static INetworkService NetworkService { get; private set; } = null!;

    // Services
    public static IClientService ClientService { get; private set; } = null!;
    public static ICounselorService CounselorService { get; private set; } = null!;
    public static IEmployerService EmployerService { get; private set; } = null!;
    public static IServiceAuthorizationService SAService { get; private set; } = null!;
    public static IStateService StateService { get; private set; } = null!;
    public static IReminderService ReminderService { get; private set; } = null!;
    public static IPlacementService PlacementService { get; private set; } = null!;

    private static ClientSelectionService? _clientSelectionService;

    // ViewModels
    public static ClientsViewModel ClientsViewModel { get; private set; } = null!;
    public static CounselorsViewModel CounselorsViewModel { get; private set; } = null!;
    public static EmployersViewModel EmployersViewModel { get; private set; } = null!;
    public static ServiceAuthorizationsViewModel POsViewModel { get; private set; } = null!;
    public static RemindersViewModel RemindersViewModel { get; private set; } = null!;
    public static StatesViewModel StatesViewModel { get; private set; } = null!;
    public static PlacementsViewModel PlacementsViewModel { get; private set; } = null!;

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
        SAService = new ServiceAuthorizationService(NetworkService);
        ReminderService = new ReminderService(NetworkService);
        StateService = new StateService(NetworkService);
        PlacementService = new PlacementService(NetworkService);

        _clientSelectionService = new();

        // Initialize ViewModels
        ClientsViewModel = new ClientsViewModel(ClientService, _clientSelectionService);
        CounselorsViewModel = new CounselorsViewModel(CounselorService);
        EmployersViewModel = new EmployersViewModel(EmployerService);
        POsViewModel = new ServiceAuthorizationsViewModel(SAService);
        RemindersViewModel = new RemindersViewModel(ReminderService, _clientSelectionService);
        StatesViewModel = new StatesViewModel(StateService);
        PlacementsViewModel = new PlacementsViewModel(PlacementService);

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
            PlacementsViewModel.LoadPlacementsAsync()
        };

        await Task.WhenAll(tasks);
        sw.Stop();
        Debug.WriteLine($"LoadDataAsync completed in {sw.ElapsedMilliseconds}");
        return true;
    }

    public static CreateCounselorViewModel CreateCounselorViewModel() {
        return new CreateCounselorViewModel(CounselorService);
    }

    public static CreateClientViewModel CreateClientViewModel() {
        return new CreateClientViewModel(ClientService);
    }

    public static CreateEmployerViewModel CreateEmployerViewModel() {
        return new CreateEmployerViewModel(EmployerService);
    }

    public static CreatePlacementViewModel CreatePlacementViewMdoel() {
        return new CreatePlacementViewModel(PlacementService);
    }

    public static CreateReminderViewModel CreateReminderViewModel(int clientId) {
        return new CreateReminderViewModel(ReminderService, clientId);
    }

    public static CreateServiceAuthorizationsViewModel CreateServiceAuthorizationsViewModel() {
        return new CreateServiceAuthorizationsViewModel(SAService);
    }
}
