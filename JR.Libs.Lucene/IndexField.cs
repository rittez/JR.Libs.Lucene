using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace JR.Libs.Lucene
{
    public class IndexField
    {
        public string FieldName { get; set; }
        public bool? IsIndexed { get;set; }
        public bool IsStored { get; set; }
        public bool? IsTermVector { get; set; }

        public string IndexName
        {
            get
            {
                if (_definition == null)
                    return FieldName;

                return string.Format("{0}.{1}", _definition.ClassName, FieldName);
            }
        }

        private  readonly IndexDefinition _definition = null;
        public IndexField(IndexDefinition definition)
        {
            _definition = definition;
        }
    }
}
