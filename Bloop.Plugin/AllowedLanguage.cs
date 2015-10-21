namespace Bloop.Plugin
{
    public static class AllowedLanguage
    {
        public static string CSharp
        {
            get { return "CSHARP"; }
        }

        public static string IronPython
        {
            get { return "PYTHON"; }
        }

        public static bool IsAllowed(string language)
        {
            return language.ToUpper() == CSharp.ToUpper() || language.ToUpper() == IronPython.ToUpper();
        }
    }
}