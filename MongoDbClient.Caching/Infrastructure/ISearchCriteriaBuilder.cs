namespace MongoDbClient.Caching.Infrastructure
{
    public interface ISearchCriteriaBuilder<in T>
    {
        SearchCriteria Build(T source);
    }
}