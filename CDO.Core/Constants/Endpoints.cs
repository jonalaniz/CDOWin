namespace CDO.Core.Constants;

public static class Endpoints {

    // -----------------------------
    // API Endpoints
    // -----------------------------

    // Client Endpoints
    public const string Clients = "/api/clients";
    public static string Client(int id) => $"{Clients}/{id}";
    public static string ClientMarkActive(int id) => $"{Clients}/{id}/mark-active";
    public static string ClientMarkInactive(int id) => $"{Clients}/{id}/mark-inactive";

    public static string ClientMarkTTW(int id) => $"{Clients}/{id}/mark-ttw";
    public static string ClientUnmarkTTW(int id) => $"{Clients}/{id}/unmark-ttw";

    public static string Note(int id) => $"{Clients}/{id}/notes";
    public static string Note(int clientId, int noteId) => $"{Clients}/{clientId}/notes/{noteId}";

    // Counselor Endpoints
    public const string Counselors = "/api/counselors";
    public static string Counselor(int id) => $"{Counselors}/{id}";

    // Employer Endpoints
    public const string Employers = "/api/employers";
    public static string Employer(int id) => $"{Employers}/{id}";
    public const string EmployerSummaries = "/api/employers/summaries";

    // Service Authorization Endpoints
    public const string ServiceAuthorizations = "/api/sas";
    public static string ServiceAuthorization(int id) => $"{ServiceAuthorizations}/{id}";
    public static string SAMarkBilled(int id) => $"{ServiceAuthorizations}/{id}/mark-billed";
    public static string SAMarkUnbilled(int id) => $"{ServiceAuthorizations}/{id}/mark-unbilled";

    // Placement Endpoints
    public const string Placements = "/api/placements";
    public static string Placement(int id) => $"{Placements}/{id}";

    // Reminder Endpoints
    public const string Reminders = "/api/reminders";
    public static string Reminder(int id) => $"{Reminders}/{id}";

    // States Endpoints
    public const string States = "/api/states";

    // Session Endpoints
    public const string Session = "/api/session";

    // -----------------------------
    // Administrative Endpoints
    // -----------------------------

    // Base Endpoint
    public const string Admin = "/api/admin";

    // Client Endpoints
    private const string AdminClients = $"{Admin}/clients";
    public const string AdminAllClientSummaries = $"{AdminClients}/all";
    public const string AdminClientExport = $"{AdminClients}/export";
    public const string AdminStaleClients = $"{AdminClients}/stale";
    public static string ClientHistory(int id) => $"{AdminClients}/{id}";

    // Notes Endpoints
    private const string AdminNotes = $"{Admin}/notes";
    public static string AdminUserNotes(string author) => $"{AdminNotes}/{author}";

    // Recent Data Endpoint: Folded recently updated Clients, Notes, and Reminders
    public static string DailySnapshot(string date) => $"{Admin}/recent?date={date}";

    // Billing Endpoints
    private const string Billing = $"{Admin}/billing";
    public const string BillingSAs = $"{Billing}/sas";
    public const string BillingExpiringSAs = $"{BillingSAs}/expiring";
    public const string BillingRecentSAs = $"{BillingSAs}/recent";
    public const string BillingNewPlacements = $"{Billing}/placements/new";

    // Users Endpoints
    public const string Users = $"{Admin}/users";
    public static string User(string id) => $"{Users}/{id}";
}
