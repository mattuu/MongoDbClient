using System.Collections.Generic;

namespace MongoDbClient.Caching.Infrastructure
{
    public interface IIndexList<out TModel>
    {
        IEnumerable<TModel> List();
    }
}