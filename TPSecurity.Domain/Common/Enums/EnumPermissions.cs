namespace TPSecurity.Domain.Common.Enums
{
    public static class EnumPermissions
    {
        private static List<string> lValue = new List<string>() { "Allow", "Restrict", "Read", "Write" };

        public static bool IsAllowedValue(string value)
        {
            return lValue.Any(a => a.ToLower() == value.Trim().ToLower());
        }
    }
}
