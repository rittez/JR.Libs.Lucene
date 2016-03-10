using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace JR.Libs.Lucene
{
    public class SearchQuery
    {
        public string SearchText { get; set; }
        public string Term { get; set; }
        public IndexDefinition SearchDefinition { get; private set; }
        public int NumItem { get; private set; }

        public SearchQuery(string text, string term, IndexDefinition searchDefinition, int numItem)
        {
            NumItem = numItem;
            SearchText = text;
            Term = term;
            SearchDefinition = searchDefinition;
        }
    }
}
