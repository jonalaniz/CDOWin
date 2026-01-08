using CDO.Core.Interfaces;
using CDO.Core.Models;
using CDO.Core.Services;
using CDOWin.Data;
using CDOWin.Navigation;
using CDOWin.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CDOWin.Services;

public static class AppServices {
    // Network
    public static INetworkService NetworkService { get; private set; } = null!;

    // Navigation
    public static INavigationService Navigation { get; } = new NavigationService();

    // Services (Network-based)
    public static IClientService ClientService { get; private set; } = null!;
    public static ICounselorService CounselorService { get; private set; } = null!;
    public static IEmployerService EmployerService { get; private set; } = null!;
    public static IServiceAuthorizationService SAService { get; private set; } = null!;
    public static IStateService StateService { get; private set; } = null!;
    public static IReminderService ReminderService { get; private set; } = null!;
    public static IPlacementService PlacementService { get; private set; } = null!;

    // Data Coordinator
    public static DataCoordinator DataCoordinator { get; private set; } = null!;

    private static ClientSelectionService? _clientSelectionService;

    private static PlacementSelectionService? _placementSelectionService;

    private static SASelectionService? _sASelectionService;

    // ViewModels
    public static CalendarViewModel CalendarViewModel { get; private set; } = null!;
    public static ClientsViewModel ClientsViewModel { get; private set; } = null!;
    public static CounselorsViewModel CounselorsViewModel { get; private set; } = null!;
    public static EmployersViewModel EmployersViewModel { get; private set; } = null!;
    public static ServiceAuthorizationsViewModel SAsViewModel { get; private set; } = null!;
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

        // Inject Services into DataCoordinator
        DataCoordinator = new DataCoordinator(
            ClientService,
            CounselorService,
            EmployerService,
            PlacementService,
            ReminderService,
            SAService,
            StateService
        );

        _clientSelectionService = new();
        _placementSelectionService = new();
        _sASelectionService = new();

        // Initialize ViewModels
        ClientsViewModel = new ClientsViewModel(
            ClientService,
            DataCoordinator,
            _clientSelectionService,
            _placementSelectionService,
            _sASelectionService
            );

        CounselorsViewModel = new CounselorsViewModel(DataCoordinator, CounselorService);
        EmployersViewModel = new EmployersViewModel(DataCoordinator, EmployerService);
        SAsViewModel = new ServiceAuthorizationsViewModel(DataCoordinator, SAService, _sASelectionService);
        RemindersViewModel = new RemindersViewModel(DataCoordinator, ReminderService, _clientSelectionService);
        StatesViewModel = new StatesViewModel(DataCoordinator, StateService);
        PlacementsViewModel = new PlacementsViewModel(DataCoordinator, PlacementService, _placementSelectionService);
        CalendarViewModel = new CalendarViewModel(RemindersViewModel);

    }

    public static async Task<bool> LoadDataAsync() {
        var sw = Stopwatch.StartNew();

        var tasks = new List<Task> {
            DataCoordinator.GetClientsAsync(),
            DataCoordinator.GetCounselorsAsync(),
            DataCoordinator.GetEmployersAsync(),
            DataCoordinator.GetRemindersAsync(),
        };

        await Task.WhenAll(tasks);

        _ = LoadSecondaryDataAsync();
        sw.Stop();
        Debug.WriteLine($"LoadDataAsync completed in {sw.ElapsedMilliseconds}");
        return true;
    }

    public static async Task LoadSecondaryDataAsync() {
        _ = DataCoordinator.GetSAsAsync();
        _ = DataCoordinator.GetPlacementsAsync();
        _ = DataCoordinator.GetSAsAsync();
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

    public static CreatePlacementViewModel CreatePlacementViewMdoel(Client client, Employer employer) {
        return new CreatePlacementViewModel(PlacementService, client, employer);
    }

    public static CreateReminderViewModel CreateReminderViewModel(int clientId) {
        return new CreateReminderViewModel(ReminderService, clientId);
    }

    public static CreateServiceAuthorizationsViewModel CreateServiceAuthorizationsViewModel(Client client) {
        return new CreateServiceAuthorizationsViewModel(SAService, client);
    }
}
