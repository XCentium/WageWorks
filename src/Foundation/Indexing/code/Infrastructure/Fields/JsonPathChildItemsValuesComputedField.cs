using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
using System;
using System.Linq;
using System.Xml;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class JsonPathChildItemsValuesComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathChildItemsValuesComputedField(XmlNode configNode) : base(configNode) { }

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
                return TryParse(results, true);
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name}", ex, this);
                return null;
            }
        }
    }
}