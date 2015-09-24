namespace Bloop.Core.Exception
{
    /// <summary>
    /// Base Bloop Exceptions
    /// </summary>
    public class BloopException : System.Exception
    {
        public BloopException(string msg)
            : base(msg)
        {

        }

        public BloopException(string msg, System.Exception innerException)
            : base(msg, innerException)
        {

        }
    }
}
