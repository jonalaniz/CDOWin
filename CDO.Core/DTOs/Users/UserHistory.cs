using CDO.Core.DTOs.Activities;

namespace CDO.Core.DTOs.Users;

public record class UserHistory(
        string Id,
        string Username,
        bool Admin,
        bool Active,
        UserActivity[] Activities
    );
