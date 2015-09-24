namespace Bloop.Core.Exception
{
    /// <summary>
    /// Represent exceptions that Bloop can't handle and MUST close running Bloop.
    /// </summary>
    public class BloopCritialException : BloopException
    {
        public BloopCritialException(string msg) : base(msg)
        {
        }
    }
}
