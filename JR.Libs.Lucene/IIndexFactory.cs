using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.Libs.Lucene
{
    public interface IIndexFactory
    {
        void BuildIndex<T>(List<T> objects, IndexDefinition definitionDefinition) where T : new();
        void AddIndex<T>(T obj, IndexDefinition definitionDefinition) where T : new();
        SearchResult Retrieve(SearchQuery searchQuery);
    }
}
