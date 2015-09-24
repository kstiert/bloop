namespace Bloop.Plugin.Features
{
    public interface IExclusiveQuery
    {
        bool IsExclusiveQuery(Query query);
    }
}
