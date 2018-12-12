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
    public class JsonPathChildItemsMinValueComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathChildItemsMinValueComputedField(XmlNode configNode) : base(configNode) { }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = base.GetItem(indexable);
            if (item == null)
            {
                return null;
            }

            try
            {
                //if (ReturnType == "datetime")
                //{
                //    var dateResults = base.GetChildItemsTokenDateTimeValues(item, base.JsonFieldName, base.JsonPath);
                //    if (dateResults == null || !dateResults.Any())
                //    {
                //        return null;
                //    }

                //    return dateResults.Min();
                //}

                var results = base.GetChildItemsTokenValues(item, base.JsonFieldName, base.JsonPath);
                if (results == null || !results.Any())
                {
                    return null;
                }

                return base.TryParse(results.Min(), true);
            }
            catch (System.Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name},", ex, this);
                return null;
            }

        }
    }
}