using CDO.Core.Interfaces;
using CDO.Core.Interfaces.Admin;

namespace Backstage.Services;

public static class AppServices {
    // Network
    public static INetworkService NetworkService { get; private set; } = null!;

    // Navigation
    // TODO: Create a navigation service

    // Services (Network-based)
    public static IAdminService AdminService { get; private set; } = null!;
    public static IUserService UserService { get; private set; } = null!;

    // ViewModels
    // TODO: Create view models
}
