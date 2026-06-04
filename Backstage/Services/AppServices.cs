using Backstage.Navigation;
using Backstage.Views;
using CDO.Abstractions.Navigation;
using CDO.Core.Interfaces;
using CDO.Core.Interfaces.Admin;

namespace Backstage.Services;

public static class AppServices {
    // Network
    public static INetworkService NetworkService { get; private set; } = null!;

    // Navigation
    public static INavigationService<BackstageView> Navigation { get; } = new NavigationService();

    // Services (Network-based)
    public static IAdminService AdminService { get; private set; } = null!;
    public static IUserService UserService { get; private set; } = null!;

    // ViewModels
    // TODO: Create view models
}
