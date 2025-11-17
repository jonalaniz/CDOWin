using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CDO.Core.Constants;

public static class Endpoints {
    public static string Client(int id) => $"{Clients}/{id}";
    public static readonly string Clients = "/api/clients";
    public static string Counselor(int id) => $"{Counselors}/{id}";
    public static readonly string Counselors = "/api/counselors";
    public static string Employer(int id) => $"{Employers}/{id}";
    public static readonly string Employers = "/api/employers";
    public static string PO(string id) => $"{POs}/{id}";
    public static readonly string POs = "/api/pos";
    public static string Referral(string id) => $"{Referrals}/{id}";
    public static readonly string Referrals = "/api/referrals";
    public static string Reminder(int id) => $"{Reminders}/{id}";
    public static readonly string Reminders = "/api/reminders";
    public static string State(int id) => $"{States}/{id}";
    public static readonly string States = "/api/states";
}
