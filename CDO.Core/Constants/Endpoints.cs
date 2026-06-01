namespace CDO.Core.Constants;

public static class Endpoints {
    
    // -----------------------------
    // API Endpoints
    // -----------------------------
    public static string Client(int id) => $"{Clients}/{id}";
    public static readonly string Clients = "/api/clients";
    public static string Note(int id) => $"{Clients}/{id}/notes";
    public static string Note(int id, int noteId) => $"{Clients}/{id}/notes/{noteId}/";
    public static string Counselor(int id) => $"{Counselors}/{id}";
    public static readonly string Counselors = "/api/counselors";
    public static string Employer(int id) => $"{Employers}/{id}";
    public static readonly string Employers = "/api/employers";
    public static readonly string EmployerSummaries = "/api/employers/summaries";
    public static string ServiceAuthorization(int id) => $"{ServiceAuthorizations}/{id}";
    public static readonly string ServiceAuthorizations = "/api/sas";
    public static string Placement(int id) => $"{Placements}/{id}";
    public static readonly string Placements = "/api/placements";
    public static string Reminder(int id) => $"{Reminders}/{id}";
    public static readonly string Reminders = "/api/reminders";
    public static string State(int id) => $"{States}/{id}";
    public static readonly string States = "/api/states";
    
    // -----------------------------
    // Administrative Endpoints
    // -----------------------------
    
    // Base Endpoint
    public static readonly string Admin = "/api/admin";
    
    // Clients: Base endpoint returns clients updated in the past 24 hours or
    // specific date if date is appended as parameter
    public static readonly string AdminClients = $"{Admin}/clients";
    public static readonly string AdminAllClients = $"{AdminClients}/all";
    public static readonly string AdminStaleClients = $"{AdminClients}/stale";
    
    // Reminders: Base endpoint returns reminders updated in the past 24 hours or
    // specific date if date is appended as parameter
    public static readonly string AdminReminders = $"{Admin}/reminders";
    
    // Notes: Base endpoint returns notes updated in the past 24 hours or
    // specific date if date is appended as parameter
    public static readonly string AdminNotes = $"{Admin}/notes";
    public static string AdminUserNotes(string author) => $"{AdminNotes}/{author}";
    
    // Users: Full endpoints, base returns all users as summaries
    public static readonly string Users = $"{Admin}/users";
    public static string User(string id) => $"{Users}/{id}";
}
