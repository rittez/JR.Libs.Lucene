using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace JR.Libs.Lucene
{
    public class IndexDefinition
    {
        private readonly string _className = string.Empty;        
        public string ClassName
        {
            get
            {
                return _className;
            }
        }

        private readonly string _indexFolder = string.Empty;
        public string IndexFolder
        {
            get
            {
                return _indexFolder;
            }
        }

        public List<IndexField> Fields
        {
            get; private set;
        }

        public IndexDefinition(string className, string indexFolder)
        {
            _className = className;
            _indexFolder = indexFolder;
            if (string.IsNullOrEmpty(_indexFolder))
            {
                _indexFolder = _className;
            }

            Fields = new List<IndexField>();
        }                   
    }
}
