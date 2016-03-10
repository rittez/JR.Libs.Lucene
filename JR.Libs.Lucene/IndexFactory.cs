using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace JR.Libs.Lucene
{
    public class IndexFactory:IIndexFactory
    {
        #region Singleton
        private static IIndexFactory _instance = null;
        private static object _lockObj = new object();

        private IndexFactory()
        {
            
        }

        private string _rootFolder = string.Empty;

        public static IIndexFactory Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new IndexFactory();
                    }

                    return _instance;
                }
            }
        }

        #endregion


        #region IIndexFactory
        private const Version LUCENE_VERSION = Version.LUCENE_29;
        public const string SCORE_FIELD = "__SCORE";

        public void BuildIndex<T>(List<T> objects, IndexDefinition definitionDefinition) where T : new()
        {
            if (definitionDefinition == null || string.IsNullOrEmpty(definitionDefinition.IndexFolder)
                || definitionDefinition.Fields.Count == 0)
            {
                throw new Exception("Invalid Doc Definition");
            }

            Directory directory = FSDirectory.Open(definitionDefinition.IndexFolder);
            Analyzer analyzer = new StandardAnalyzer(LUCENE_VERSION);
            using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var fieldNames = definitionDefinition.Fields.Select(a => a.FieldName).ToArray();                
                foreach (var o in objects)
                {
                    var serializedObj = o.ToDictionary(fieldNames);
                    var doc = new Document();
                    foreach (var field in definitionDefinition.Fields)
                    {
                        var store = field.IsStored ? Field.Store.YES : Field.Store.NO;
                        var index = !field.IsIndexed.HasValue
                            ? Field.Index.NO
                            : (field.IsIndexed == true ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED);                       
                        var f = new Field(field.IndexName, serializedObj[field.FieldName],store, index);
                        doc.Add(f);
                    }
                    
                    writer.AddDocument(doc);
                }

                writer.Optimize();                   
            }
        }

        public void AddIndex<T>(T obj, IndexDefinition definitionDefinition) where T : new()
        {
            if (definitionDefinition == null || string.IsNullOrEmpty(definitionDefinition.IndexFolder)
                || definitionDefinition.Fields.Count == 0)
            {
                throw new Exception("Invalid Doc Definition");
            }

            Directory directory = FSDirectory.Open(definitionDefinition.IndexFolder);
            Analyzer analyzer = new StandardAnalyzer(LUCENE_VERSION);
            using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var fieldNames = definitionDefinition.Fields.Select(a => a.FieldName).ToArray();
                var serializedObj = obj.ToDictionary(fieldNames);
                var doc = new Document();
                foreach (var field in definitionDefinition.Fields)
                {
                    var store = field.IsStored ? Field.Store.YES : Field.Store.NO;
                    var index = !field.IsIndexed.HasValue
                        ? Field.Index.NO
                        : (field.IsIndexed == true ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED);
                    var f = new Field(field.IndexName, serializedObj[field.FieldName], store, index);
                    doc.Add(f);
                }

                writer.AddDocument(doc);

                writer.Optimize();
            }
        }

        public SearchResult Retrieve(SearchQuery query)
        {
            if (query == null || string.IsNullOrEmpty(query.SearchText) || string.IsNullOrEmpty(query.Term))
            {
                throw new Exception("Invalid query");
            }

            var analyzer = new StandardAnalyzer(LUCENE_VERSION);
            var parser = new QueryParser(LUCENE_VERSION, query.Term, analyzer);
            var pquery = parser.Parse(query.SearchText);

            var directory = FSDirectory.Open(query.SearchDefinition.IndexFolder);
            var searcher = new IndexSearcher(IndexReader.Open(directory, true));
            var collector = TopScoreDocCollector.Create(query.NumItem, true);

            searcher.Search(pquery, collector);
            ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
            var rs = new SearchResult();
            for (int i = 0; i < hits.Length; i++)
            {
                int docId = hits[i].Doc;
                float score = hits[i].Score;
                var doc = searcher.Doc(docId);
                var rsItem = new Dictionary<string, string>();
                rsItem.Add(SCORE_FIELD, score.ToString());
                foreach (var field in query.SearchDefinition.Fields)
                {
                    rsItem.Add(field.FieldName, doc.Get(field.IndexName));
                }

                rs.Results.Add(rsItem);
            }

            return rs;
        }
        #endregion
    }
}