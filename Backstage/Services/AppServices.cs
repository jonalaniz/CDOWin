using Backstage.Data;
using Backstage.Navigation;
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
    public static BillingService BillingService { get; private set; } = null!;
    public static AdminClientService ClientService { get; private set; } = null!;
    public static AdminReminderService ReminderService { get; private set; } = null!;
    public static UserService UserService { get; private set; } = null!;

    // Data Coordination
    public static DataCoordinator DataCoordinator { get; private set; } = null!;

    // ViewModels
    // TODO: Create view models

    public static void InitializeServices(string baseAddress, string apiKey) {
        // Initialize network service
        var network = new NetworkService();
        network.Initialize(baseAddress, apiKey);
        NetworkService = network;

        // Initialize child services
        BillingService = new BillingService(network);
        ClientService = new AdminClientService(network);
        ReminderService = new AdminReminderService(network);
        UserService = new UserService(network);

        // Inject Services into DataCoordinator
        DataCoordinator = new DataCoordinator(
            BillingService,
            ClientService,
            ReminderService,
            UserService
            );
    }

    public static async Task<bool> LoadDataAsync() {
        var sw = Stopwatch.StartNew();

        var tasks = new List<Task> {
            DataCoordinator.GetUsersAsync()
        };

        await Task.WhenAll(tasks);

        // _ = LoadSecondaryDataAsync();
        sw.Stop();
        Debug.WriteLine($"LoadDataAsync completed in {sw.ElapsedMilliseconds}");
        return true;
    }
}
