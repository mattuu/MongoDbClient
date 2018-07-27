using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MongoDbClient.Caching.Infrastructure
{
    public class IndexListBase<TModel> : IndexManagerBase<TModel>
    {
        private readonly IDocumentToModelMapper<TModel> _documentToModelMapper;

        public IndexListBase(IConfiguration configuration,
                             Func<TModel, string> keyAccessorFunc,
                             string keyFieldName,
                             ILogger logger,
                             IDocumentToModelMapper<TModel> documentToModelMapper)
            : base(configuration, keyAccessorFunc, keyFieldName, logger)
        {
            _documentToModelMapper = documentToModelMapper ?? throw new ArgumentNullException(nameof(documentToModelMapper));
        }

        public virtual IEnumerable<TModel> List()
        {
            var query = new MatchAllDocsQuery();

            using (var searcher = new IndexSearcher(Directory, true))
            {
                var filter = new QueryWrapperFilter(query);

                var hits = searcher.Search(query, filter, int.MaxValue).ScoreDocs;

                Logger.LogDebug($"Listing Lucene index returned {hits.Length} documents");

                return hits.Select(h => _documentToModelMapper.Map(searcher.Doc(h.Doc))).ToList();
            }
        }
    }
}