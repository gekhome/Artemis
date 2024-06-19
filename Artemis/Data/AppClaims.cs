namespace Artemis.Data
{
    public static class AppClaims
    {
        public static List<string> ClaimTypes = new List<string>()
        {
            "Admin", "User", "Guest", "VIP", "Disabled"
        };
    }
}
