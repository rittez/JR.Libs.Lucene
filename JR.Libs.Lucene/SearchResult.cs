using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.Libs.Lucene
{
    public class SearchResult
    {
        public List<Dictionary<string, string>> Results
        {
            get; private set;
        }

        public SearchResult()
        {
            Results = new List<Dictionary<string, string>>();
        }
    }
}
