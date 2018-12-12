using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
using System.Linq;
using System.Xml;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Get min value from 
    /// </summary>
    public class JsonPathChildItemsMaxValueComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathChildItemsMaxValueComputedField(XmlNode configNode) : base(configNode) { }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = base.GetItem(indexable);
            if (item == null)
            {
                return null;
            }

            try
            {
                var results = base.GetChildItemsTokenValues(item, base.JsonFieldName, base.JsonPath);
                if (results == null || results.Count() == 0)
                {
                    return null;
                }

                return results.Max();
            }
            catch (System.Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name},", ex, this);
                return null;
            }
        }
    }
}