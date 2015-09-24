using System.Windows;
using System.Windows.Input;

namespace Bloop.Plugin
{
    public delegate void BloopKeyDownEventHandler(BloopKeyDownEventArgs e);
    public delegate void AfterBloopQueryEventHandler(BloopQueryEventArgs e);

    public delegate void ResultItemDropEventHandler(Result result, IDataObject dropObject, DragEventArgs e);

    /// <summary>
    /// Global keyboard events
    /// </summary>
    /// <param name="keyevent">WM_KEYDOWN = 256,WM_KEYUP = 257,WM_SYSKEYUP = 261,WM_SYSKEYDOWN = 260</param>
    /// <param name="vkcode"></param>
    /// <param name="state"></param>
    /// <returns>return true to continue handling, return false to intercept system handling</returns>
    public delegate bool BloopGlobalKeyboardEventHandler(int keyevent, int vkcode, SpecialKeyState state);

    public class BloopKeyDownEventArgs
    {
        public string Query { get; set; }
        public KeyEventArgs keyEventArgs { get; set; }
    }

    public class BloopQueryEventArgs
    {
        public Query Query { get; set; }
    }
}
