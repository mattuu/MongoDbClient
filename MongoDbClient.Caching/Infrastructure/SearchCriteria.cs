using Lucene.Net.Search;

namespace MongoDbClient.Caching.Infrastructure
{
    public class SearchCriteria
    {
        public Query Query { get; set; }

        public Sort Sort { get; set; }
    }
}