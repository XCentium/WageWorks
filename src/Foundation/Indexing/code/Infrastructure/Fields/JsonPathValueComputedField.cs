using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class JsonPathValueComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathValueComputedField (XmlNode configNode) : base(configNode)
        {
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = GetItem(indexable);
            if (item == null)
            {
                return null;
            }

            try
            {
                var values = GetTokenValues(item, base.JsonFieldName, base.JsonPath);
                if (values == null || values.Count() == 0)
                {
                    return null;
                }

                if (values.Count() > 1)
                {
                    return (this.ArrayPosition != -1) ? 
                        TryParse(values.ElementAtOrDefault(this.ArrayPosition), true) 
                        : TryParse(values, true);
                }

                var firstValue = values.FirstOrDefault();
                if (!ReturnType.Equals("string", StringComparison.OrdinalIgnoreCase) && !ReturnType.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    return TryParse(firstValue, true);
                }

                if (this.SortFolder == Guid.Empty) return firstValue;

                ChildList sortValues = base.GetSortItems();
                if (sortValues == null)
                {
                    return base.TryParse(firstValue, true);
                }

                if (sortValues != null)
                {
                    //Find sort Value with same value as facet
                    var sortItem = sortValues.FirstOrDefault(facet => facet["Value"] == firstValue);
                    if (sortItem != null)
                        firstValue = !string.IsNullOrWhiteSpace(sortItem[Sitecore.FieldIDs.Sortorder])
                            ? sortItem[Sitecore.FieldIDs.Sortorder]
                            : "100000";
                }

                return base.TryParse(firstValue, true);
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name}", ex, this);
                return null;
            }
        }


    }
}