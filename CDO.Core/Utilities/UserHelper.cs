namespace CDO.Core.Utilities;

public static class UserHelper {
    public static String UsernameQuery => $"?name={Username}";
    public static String Username => char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];
}
