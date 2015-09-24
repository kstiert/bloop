using System.Diagnostics;

namespace Bloop.Infrastructure
{
    public static class DebugHelper
    {
        public static void WriteLine(string msg)
        {
#if DEBUG
            Debug.WriteLine(msg);
#else
            return;
#endif
        }
    }
}
