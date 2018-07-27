namespace MongoDbClient.Caching.Infrastructure
{
    public interface IIndexManager<TModel>
    {
        void Optimize();
    }
}