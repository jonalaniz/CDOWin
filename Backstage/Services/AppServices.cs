using Backstage.Data;
using Backstage.Navigation;
using Backstage.ViewModels;
using Backstage.Views;
using CDO.Abstractions.Navigation;
using CDO.Core.Interfaces;
using CDO.Core.Services;
using CDO.Core.Services.Admin;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backstage.Services;

public static class AppServices {
    // Network
    public static INetworkService NetworkService { get; private set; } = null!;

    // Navigation
    public static INavigationService<BackstageView> Navigation { get; } = new NavigationService();

    // Services (Network-based)
    public static HomeViewModel HomeViewModel { get; private set; } = null!;
    public static BillingService BillingService { get; private set; } = null!;
    public static AdminClientService AdminClientService { get; private set; } = null!;
    public static ClientService ClientService { get; private set; } = null!;
    public static AdminReminderService AdminReminderService { get; private set; } = null!;
    public static ReminderService ReminderService { get; private set; } = null!;
    public static UserService UserService { get; private set; } = null!;

    // Data Coordination
    public static DataCoordinator DataCoordinator { get; private set; } = null!;

    // ViewModels
    public static BillingViewModel BillingViewModel { get; private set; } = null!;
    public static ClientViewModel ClientViewModel { get; private set; } = null!;
    public static UserViewModel UserViewModel { get; private set; } = null!;

    public static void InitializeServices(string baseAddress, string apiKey) {
        // Initialize network service
        var network = new NetworkService();
        network.Initialize(baseAddress, apiKey);
        NetworkService = network;

        // Initialize child services
        BillingService = new BillingService(network);
        AdminClientService = new AdminClientService(network);
        ClientService = new ClientService(network);
        AdminReminderService = new AdminReminderService(network);
        ReminderService = new ReminderService(network);
        UserService = new UserService(network);

        // Inject Services into DataCoordinator
        DataCoordinator = new DataCoordinator(
            BillingService,
            AdminClientService,
            AdminReminderService,
            UserService
            );

        // Initialize ViewModels
        HomeViewModel = new HomeViewModel(
            DataCoordinator,
            ClientService,
            ReminderService
            );

        BillingViewModel = new BillingViewModel(
            DataCoordinator,
            BillingService
            );

        ClientViewModel = new ClientViewModel(
            DataCoordinator,
            AdminClientService
            );

        UserViewModel = new UserViewModel(
            DataCoordinator,
            UserService
            );
    }

    public static async Task<bool> LoadDataAsync() {
        var sw = Stopwatch.StartNew();

        var tasks = new List<Task> {
            DataCoordinator.GetUsersAsync(),
            DataCoordinator.GetUnbilledSAsAsync(),
            DataCoordinator.GetUnbilledPlacementsAsync(),
            DataCoordinator.GetExpiringSAsAsync(),
            DataCoordinator.GetRecentClientsAsync(),
            DataCoordinator.GetStaleClientsAsync()
        };

        await Task.WhenAll(tasks);

        // _ = LoadSecondaryDataAsync();
        sw.Stop();
        Debug.WriteLine($"LoadDataAsync completed in {sw.ElapsedMilliseconds}");
        return true;
    }
}
