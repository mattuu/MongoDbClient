using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MongoDbClient.Caching.Infrastructure
{
    public abstract class IndexQueryBase<TModel, TQueryParameters> : IndexManagerBase<TModel>
    {
        private readonly IDocumentToModelMapper<TModel> _documentToModelMapper;
        private readonly ISearchCriteriaBuilder<TQueryParameters> _searchCriteriaBuilder;

        protected IndexQueryBase(IConfiguration configuration,
                                 Func<TModel, string> keyAccessorFunc,
                                 string keyFieldName,
                                 ILogger logger,
                                 IDocumentToModelMapper<TModel> documentToModelMapper,
                                 ISearchCriteriaBuilder<TQueryParameters> searchCriteriaBuilder)
            : base(configuration, keyAccessorFunc, keyFieldName, logger)
        {
            _documentToModelMapper = documentToModelMapper ?? throw new ArgumentNullException(nameof(documentToModelMapper));
            _searchCriteriaBuilder = searchCriteriaBuilder ?? throw new ArgumentNullException(nameof(searchCriteriaBuilder));
        }

        public virtual IEnumerable<TModel> Search(TQueryParameters queryParameters)
        {
            var searchCriteria = _searchCriteriaBuilder.Build(queryParameters);

            using (var searcher = new IndexSearcher(Directory, true))
            {
                var filter = new QueryWrapperFilter(searchCriteria.Query);

                var hits = searchCriteria.Sort == null
                    ? searcher.Search(searchCriteria.Query, filter, int.MaxValue).ScoreDocs
                    : searcher.Search(searchCriteria.Query, filter, int.MaxValue, searchCriteria.Sort).ScoreDocs;

                Logger.LogDebug($"Searching Lucene index with query: {searchCriteria.Query} and sort: {searchCriteria.Sort} returned {hits.Length} documents");

                return hits.Select(h => _documentToModelMapper.Map(searcher.Doc(h.Doc))).ToList();
            }
        }

        public virtual TModel Single(TQueryParameters queryParameters)
        {
            return Search(queryParameters).FirstOrDefault();
        }
    }
}