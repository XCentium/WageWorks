using System;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Get combined list of all values for Json paths, specified in jsonPath elements in computed field configuration
    /// </summary>
    public class JsonPathCombinedValuesComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public JsonPathCombinedValuesComputedField(XmlNode configNode) : base(configNode)
        {
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = GetItem(indexable);
            if (item == null)
            {
                return null;
            }
            var resultList = new List<string>();

            if (!string.IsNullOrWhiteSpace(this.JsonPathsFolder))
            {
                var folder = item.Database.GetItem(this.JsonPathsFolder);
                var attributes = folder?.Children.ToList();
                if (attributes != null && attributes.Any())
                {
                    if (item.Template.ID == Templates.Product.ID)
                    {
                        foreach (Item child in item.Children)
                        {
                            resultList.AddRange(GetSpecifications(child, attributes));
                        }
                    }
                    else if (item.Template.ID == Templates.ProductVariant.ID)
                    {
                        resultList.AddRange(GetSpecifications(item, attributes));
                    }

                }
            }

            if (JsonPaths != null && JsonPaths.Any())
            {
                foreach (var jsonPath in JsonPaths)
                {
                    var results = GetChildItemsTokenValues(item, base.JsonFieldName, jsonPath.Key);
                    if (results == null) continue;

                    foreach (var result in results)
                    {
                        if (result == null || resultList.Contains(result.ToString())) continue;
                        var value = result;
                        if (value.ToLowerInvariant() == "true") value = jsonPath.Value;
                        if (value.ToLowerInvariant() != "false")
                            resultList.Add(value);
                    }
                }
            }


            return resultList.Count > 0 ? resultList.Distinct() : null;
        }
        private List<string> GetSpecifications(Item item, List<Item> attributes)
        {
            var specs = new List<string>();

            var excluded = new string[] { "false", "true", "n/a", "none", "yes", "no" };

            var fieldValue = item[Templates.ProductVariant.Fields.SkuEntityJson];
            if (string.IsNullOrWhiteSpace(fieldValue)) return specs;

            var json = JObject.Parse(fieldValue);

            if (json != null)
            {
                foreach (var attribute in attributes)
                {
                    try
                    {
                        var jsonPath = attribute[Templates.ProductAttribute.Fields.ProductAttributeJsonPath];
                        if (string.IsNullOrWhiteSpace(jsonPath)) continue;

                        var tokens = json.SelectTokens(jsonPath)
                            .Where(e => e != null && !string.IsNullOrEmpty(e.Value<string>()))
                            .Select(x => x.Value<string>());

                        if (tokens == null || !tokens.Any()) continue;

                        var value = tokens.ToList<string>()?.FirstOrDefault();
                        if (value != null && !excluded.Contains(value.ToLowerInvariant()))
                            specs.Add(value);
                    }
                    catch (Exception e)
                    {
                        Log.Warn($"Could not retrieve attribute for compute field:  Item Id: {item.ID}, Name: {item.Name}," +
                                  $" Attribute: {attribute[Templates.ProductAttribute.Fields.ProductAttributeJsonPath]}", e, this);
                        continue;
                    }
                }
            }

            return specs.Distinct().ToList();

        }
    }
}