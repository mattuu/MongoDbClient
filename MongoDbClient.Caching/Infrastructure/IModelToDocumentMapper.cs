using Lucene.Net.Documents;

namespace MongoDbClient.Caching.Infrastructure
{
    public interface IModelToDocumentMapper<in TModel>
    {
        Document Map(TModel source);
    }
}