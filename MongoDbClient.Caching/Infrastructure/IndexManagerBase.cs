using System;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Version = Lucene.Net.Util.Version;

namespace MongoDbClient.Caching.Infrastructure
{
    public abstract class IndexManagerBase<TModel> : IIndexManager<TModel>
    {
        private FSDirectory _directoryTemp;

        protected IndexManagerBase(IConfiguration configuration,
                                   Func<TModel, string> keyAccessorFunc,
                                   string keyFieldName,
                                   ILogger logger)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            KeyAccessorFunc = keyAccessorFunc ?? throw new ArgumentNullException(nameof(keyAccessorFunc));
            KeyFieldName = keyFieldName ?? throw new ArgumentNullException(nameof(keyFieldName));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected internal string LuceneDirConfigKey => "Caching:LuceneDirectory";

        private IConfiguration Configuration { get; }

        protected internal string LuceneDir => Path.Combine(Configuration[LuceneDirConfigKey], "lucene_index");

        protected ILogger Logger { get; }

        protected Func<TModel, string> KeyAccessorFunc { get; }

        protected string KeyFieldName { get; }

        protected FSDirectory Directory
        {
            get
            {
                if (_directoryTemp == null)
                {
                    _directoryTemp = GetFSDirectory();
                }
                if (IndexWriter.IsLocked(_directoryTemp))
                {
                    IndexWriter.Unlock(_directoryTemp);
                }

                var lockFilePath = Path.Combine(LuceneDir, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }
                return _directoryTemp;
            }
        }


        public void Optimize()
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                }
            }
        }

        private FSDirectory GetFSDirectory()
        {
            var directoryInfo = new DirectoryInfo(LuceneDir);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            return FSDirectory.Open(directoryInfo);
        }
    }
}