using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;
using System;
using System.Linq;

namespace WageWorks.Foundation.Indexing.Infrastructure.Readers
{
    public class PipeDelimetedListFieldReader : FieldReader
    {
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            Field field = indexableField as SitecoreItemDataField;
            if (field == null)
            {
                return string.Empty;
            }
            return (field.Value ?? string.Empty).Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}