using CDOWin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDO.Core.Services;

public static class AppServices {
    // Network Service singleton
    public static INetworkService? NetworkService { get; private set; }

    // Services
    public static IClientService? ClientService { get; private set; }
    public static IEmployerService? EmployerService { get; private set; }
    public static IStateService? StateService { get; private set; }

    // ViewModels
    public static ClientsViewModel ClientsViewModel { get; private set; }
    public static EmployersViewModel EmployersViewModel { get; private set; }

    // Initialize all services
    public static void InitializeServices(string baseAddress, string apiKey) {
        // Initialize network service
        var network = new NetworkService();
        network.Initialize(baseAddress, apiKey);
        NetworkService = network;

        // Initialize other services
        ClientService = new ClientService(NetworkService);
        EmployerService = new EmployerService(NetworkService);
        StateService = new StateService(NetworkService);

        // Add more services here as needed:

        // Initialize ViewModels
        ClientsViewModel = new ClientsViewModel(ClientService);

    }

    public static async Task LoadDataAsync() {
        var tasks = new List<Task> {
            ClientsViewModel.LoadClientsAsync()
            // Add other services `InitializeAsync()` calls here
        };

        await Task.WhenAll(tasks);
    }
}
