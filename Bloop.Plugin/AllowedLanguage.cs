namespace Bloop.Plugin
{
    public static class AllowedLanguage
    {
        public static string CSharp
        {
            get { return "CSHARP"; }
        }

        public static bool IsAllowed(string language)
        {
            return language.ToUpper() == CSharp.ToUpper();
        }
    }
}