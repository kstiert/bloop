namespace Bloop.Core.Plugin.QueryDispatcher
{
    internal interface IQueryDispatcher
    {
        void Dispatch(Bloop.Plugin.Query query);
    }
}
