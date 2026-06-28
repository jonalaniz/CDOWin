namespace CDO.Core.Models;

public record class SessionToken(
    string Id,
    UserStub UserStub,
    DateTime CreatedAt,
    DateTime ExpiresAt
    );

public record class UserStub(string Id);