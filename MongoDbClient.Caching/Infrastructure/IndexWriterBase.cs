using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Version = Lucene.Net.Util.Version;

namespace MongoDbClient.Caching.Infrastructure
{
    public abstract class IndexWriterBase<TModel> : IndexManagerBase<TModel>, IIndexWriter<TModel>
    {
        private readonly IModelToDocumentMapper<TModel> _modelToDocumentMapper;

        protected IndexWriterBase(IConfiguration configuration,
                                  Func<TModel, string> keyAccessorFunc,
                                  string keyFieldName,
                                  ILogger logger,
                                  IModelToDocumentMapper<TModel> modelToDocumentMapper)
            : base(configuration, keyAccessorFunc, keyFieldName, logger)
        {
            _modelToDocumentMapper = modelToDocumentMapper ?? throw new ArgumentNullException(nameof(modelToDocumentMapper));
        }

        public void AddOrUpdateIndex(params TModel[] data)
        {
            Logger.LogDebug($"Updating Lucene index using directory: {Directory.Directory.FullName}");

            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var counter = 0;
                    foreach (var model in data)
                    {
                        var id = KeyAccessorFunc(model);

                        var searchQuery = new TermQuery(new Term(KeyFieldName, id));
                        writer.DeleteDocuments(searchQuery);

                        var doc = _modelToDocumentMapper.Map(model);

                        if (doc != null)
                        {
                            writer.AddDocument(doc);
                            counter++;
                        }
                    }

                    Logger.LogDebug($"Updated {counter} documents in index");
                }
            }
        }

        public void ClearLuceneIndexRecord(string key)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term(KeyFieldName, key));
                    writer.DeleteDocuments(searchQuery);

                    Logger.LogDebug($"Cleared Lucene index for document with key: {key}");
                }
            }
        }

        public void ClearLuceneIndex()
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    Logger.LogDebug("Cleared full Lucene index");
                }
            }
        }
    }
}