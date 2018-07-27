using Lucene.Net.Documents;

namespace MongoDbClient.Caching.Infrastructure
{
    public interface IDocumentToModelMapper<out TModel>
    {
        TModel Map(Document document);
    }
}