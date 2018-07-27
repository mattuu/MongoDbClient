namespace MongoDbClient.Caching.Infrastructure
{
    public interface IIndexWriter<in TModel>
    {
        void AddOrUpdateIndex(params TModel[] data);

        void ClearLuceneIndexRecord(string key);

        void ClearLuceneIndex();
    }
}