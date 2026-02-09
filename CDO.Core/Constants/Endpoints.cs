namespace CDO.Core.Constants;

public static class Endpoints {
    public static string Client(int id) => $"{Clients}/{id}";
    public static readonly string Clients = "/api/clients";
    public static readonly string ClientSummaries = "/api/clients/summaries";
    public static string Counselor(int id) => $"{Counselors}/{id}";
    public static readonly string Counselors = "/api/counselors";
    public static readonly string CounselorSummaries = "/api/counselors/summaries";
    public static string Employer(int id) => $"{Employers}/{id}";
    public static readonly string Employers = "/api/employers";
    public static readonly string EmployerSummaries = "/api/employers/summaries";
    public static string ServiceAuthorization(int id) => $"{ServiceAuthorizations}/{id}";
    public static readonly string ServiceAuthorizations = "/api/pos";
    public static string Placement(int id) => $"{Placements}/{id}";
    public static readonly string Placements = "/api/placements";
    public static readonly string PlacementSummaries = "/api/placements/summaries";
    public static string Reminder(int id) => $"{Reminders}/{id}";
    public static readonly string Reminders = "/api/reminders";
    public static string State(int id) => $"{States}/{id}";
    public static readonly string States = "/api/states";
}
