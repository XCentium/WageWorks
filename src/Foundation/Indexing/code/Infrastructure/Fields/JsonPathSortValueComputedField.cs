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

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class JsonPathSortValueComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathSortValueComputedField(XmlNode configNode) : base(configNode)
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

                int defaultValue = 100000;

                var facetValue = values.FirstOrDefault();

                if (this.SortFolder == Guid.Empty) return defaultValue;

                var db = Sitecore.Configuration.Factory.GetDatabase("master");
                var parentItem = db?.GetItem(new ID(this.SortFolder));
                if (parentItem == null) return defaultValue;

                var sortValues = parentItem.Children;
                //Find sort Value with same value as facet
                var sortItem = sortValues.FirstOrDefault(facet => facet["Value"] == facetValue);
                if (sortItem != null)
                    return !string.IsNullOrWhiteSpace(sortItem[Sitecore.FieldIDs.Sortorder])
                        ? MainUtil.GetInt(sortItem[Sitecore.FieldIDs.Sortorder], defaultValue)
                        : defaultValue;

                return defaultValue;
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name}", ex, this);
                return null;
            }
        }

    }
}