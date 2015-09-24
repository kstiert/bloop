namespace Bloop.Infrastructure.Storage
{
    public interface IStorage
    {
        void Load();
        void Save();
    }
}
