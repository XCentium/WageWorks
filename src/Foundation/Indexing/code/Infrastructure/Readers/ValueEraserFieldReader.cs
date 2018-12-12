using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;
using System;
using System.Linq;

namespace WageWorks.Foundation.Indexing.Infrastructure.Readers
{
    public class ValueEraserFieldReader : FieldReader
    {
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            return string.Empty;
        }
    }
}