using System.Collections.Generic;

namespace MongoDbClient.Caching.Infrastructure
{
    public interface IIndexQuery<out TModel, in TQueryParameters>
    {
        IEnumerable<TModel> Search(TQueryParameters parameters);

        TModel Single(TQueryParameters parameters);
    }
}