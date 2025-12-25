namespace CDO.Core.Constants;

public static class Endpoints {
    public static string Client(int id) => $"{Clients}/{id}";
    public static readonly string Clients = "/api/clients";
    public static readonly string ClientSummaries = "/api/clients/summaries";
    public static string Counselor(int id) => $"{Counselors}/{id}";
    public static readonly string Counselors = "/api/counselors";
    public static string Employer(int id) => $"{Employers}/{id}";
    public static readonly string Employers = "/api/employers";
    public static string ServiceAuthorization(string id) => $"{ServiceAuthorizations}/{id}";
    public static readonly string ServiceAuthorizations = "/api/pos";
    public static string Placement(string id) => $"{Placements}/{id}";
    public static readonly string Placements = "/api/placements";
    public static string Reminder(int id) => $"{Reminders}/{id}";
    public static readonly string Reminders = "/api/reminders";
    public static string State(int id) => $"{States}/{id}";
    public static readonly string States = "/api/states";
}
